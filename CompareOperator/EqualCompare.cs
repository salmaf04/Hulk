class EqualCompare : BinaryExpression
{
 
    protected override Expression LeftExpression {get;}

    protected override Expression RightExpression {get;}

    public override string value => "==";

    public override SyntaxKind Kind {get;}
    public EqualCompare(Expression LeftExpression, Expression RightExpression){
        this.LeftExpression = LeftExpression;
        this.RightExpression = RightExpression;
       
    }

    public override string Evaluate()
    {
        bool sol = (LeftExpression.Evaluate()) == (RightExpression.Evaluate());
        return sol.ToString();
    }
}