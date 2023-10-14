 class AndOperator : BinaryExpression
{
 
    protected override Expression LeftExpression {get;}

    protected override Expression RightExpression {get;}

    public override string value => "&&";

    public override SyntaxKind Kind {get;}
    public AndOperator(Expression LeftExpression, Expression RightExpression){
        this.LeftExpression = LeftExpression;
        this.RightExpression = RightExpression;
    }

    public override string Evaluate()
    {
        bool sol = (bool.Parse(LeftExpression.Evaluate()))  && bool.Parse((RightExpression.Evaluate()));
        return sol.ToString();
    }
}