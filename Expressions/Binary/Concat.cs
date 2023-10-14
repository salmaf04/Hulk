    class Concat : BinaryExpression
{
 
    protected override Expression LeftExpression {get;}

    protected override Expression RightExpression {get;}

    public override string value => "@";

    public override SyntaxKind Kind {get;}
    public Concat(Expression LeftExpression, Expression RightExpression){
        this.LeftExpression = LeftExpression;
        this.RightExpression = RightExpression;
    }

    public override string Evaluate()
    {
        return (LeftExpression.Evaluate()) + (RightExpression.Evaluate());
        
    }
}
