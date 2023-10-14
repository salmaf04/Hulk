class SyntaxToken{
    public SyntaxToken(SyntaxKind kind, int position, string text, object value){
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }
    public SyntaxKind Kind {get;}
    public int Position {get;}
    public string Text {get;}
    public object Value {get;}
}
 enum SyntaxKind{
    NumberToken,
    WordToken,
    StringToken,
    WhitespaceToken,
    PlusToken,
    MinusToken,
    AsterikToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    CaretToken,
    EqualCompareToken,
    ArrowToken,
    EqualToken,
    DifferentCompareToken,
    NegationToken,
    AtSignToken,
    PorcentToken,
    CommaToken,
    SemiColonToken,
    AndToken,
    OrToken,
    MenorIgualToken,
    MenorToken,
    MayorIgualToken,
    MayorToken,
    BadToken,
    EndofFileToken,
    PredefinedFunctionToken,
    KeyWordToken
 }