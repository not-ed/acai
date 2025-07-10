namespace AcaiMobile.ContentViews;

public partial class NoteCard : ContentView
{
    public static readonly BindableProperty NoteContentProperty = BindableProperty.Create(nameof(NoteContent), typeof(string), typeof(NoteCard), string.Empty, propertyChanged: OnNoteContentChanged);
    
    public string NoteContent
    {
        get => (string)GetValue(NoteContentProperty);
        set => SetValue(NoteContentProperty, value);
    }
    
    private static void OnNoteContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NoteCard)
        {
            ((NoteCard)bindable).FormattedLabels.ItemsSource = TokenizeAndFormatContent(newValue?.ToString());
        }
    }

    private static List<Label> TokenizeAndFormatContent(string value)
    {
        var formattedContentLabels = new List<Label>();

        if (string.IsNullOrEmpty(value))
        {
            return formattedContentLabels;
        }
        
        bool useBoldFormatting = false;
        bool useItalicFormatting = false;
        
        foreach (var line in value.Split("\\n"))
        {
            var label = new Label()
            {
                FormattedText = new FormattedString()
                {
                    Spans = { }
                }
            };

            foreach (var token in line.Split(" "))
            {
                var word = token;
                FontAttributes tokenFormatting = FontAttributes.None;

                bool charactersInWordOnlyContainAsteriskLiterals = word.Replace("*","").Length == 0;

                if (!charactersInWordOnlyContainAsteriskLiterals)
                {
                    if (word.StartsWith("**"))
                    {
                        word = word.Substring(2);
                        useBoldFormatting = !useBoldFormatting;
                    }

                    if (word.StartsWith("*"))
                    {
                        word = word.Substring(1);
                        useItalicFormatting = !useItalicFormatting;
                    }
                }

                tokenFormatting = tokenFormatting | (useBoldFormatting ? FontAttributes.Bold : FontAttributes.None) | (useItalicFormatting ? FontAttributes.Italic : FontAttributes.None);
                
                if (!charactersInWordOnlyContainAsteriskLiterals) { 
                    if (word.EndsWith("**"))
                    {
                        word = word.Substring(0, word.Length - 2);
                        useBoldFormatting = !useBoldFormatting;
                    }
                    if (word.EndsWith("*"))
                    {
                        word = word.Substring(0, word.Length - 1);
                        useItalicFormatting = !useItalicFormatting;
                    }
                }
                
                label.FormattedText.Spans.Insert(label.FormattedText.Spans.Count, new Span(){ Text = $"{word} ", FontAttributes = tokenFormatting });
            }
            
            formattedContentLabels.Insert(formattedContentLabels.Count, label);
        }

        return formattedContentLabels;
    }
    
    public NoteCard()
    {
        InitializeComponent();
    }
}