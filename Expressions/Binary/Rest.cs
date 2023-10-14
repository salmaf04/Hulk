    class Rest : BinaryExpression
{
 
    protected override Expression LeftExpression {get;}

    protected override Expression RightExpression {get;}

    public override string value => "%";

    public override SyntaxKind Kind {get;}
    public Rest(Expression LeftExpression, Expression RightExpression){
        this.LeftExpression = LeftExpression;
        this.RightExpression = RightExpression;
    }

    public override string Evaluate()
    {
        return ((double.Parse(LeftExpression.Evaluate())) % (double.Parse(RightExpression.Evaluate()))).ToString();
        
    }
}
