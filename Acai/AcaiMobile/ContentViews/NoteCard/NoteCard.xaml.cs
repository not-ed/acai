namespace AcaiMobile.ContentViews;

public partial class NoteCard : ContentView
{
    private readonly BindableProperty _contentProperty = BindableProperty.Create(nameof(Content), typeof(string), typeof(NoteCard), string.Empty);
    private readonly BindableProperty _formattedContentLabelsProperty = BindableProperty.Create(nameof(FormattedContentLabels), typeof(List<Label>), typeof(NoteCard), new List<Label>());
    
    public string Content
    {
        set => TokenizeAndFormatContent(value);
    }

    public List<Label> FormattedContentLabels
    {
        get => (List<Label>)GetValue(_formattedContentLabelsProperty);
    }

    private void TokenizeAndFormatContent(string value)
    {
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
            
            FormattedContentLabels.Insert(FormattedContentLabels.Count, label);
        }
        SetValue(_contentProperty, value);
    }
    
    public NoteCard()
    {
        InitializeComponent();
    }
}