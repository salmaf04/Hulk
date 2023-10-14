class MayorIgualOperator : BinaryExpression
{
 
    protected override Expression LeftExpression {get;}

    protected override Expression RightExpression {get;}

    public override string value => ">=";

    public override SyntaxKind Kind {get;}
    public MayorIgualOperator(Expression LeftExpression, Expression RightExpression){
        this.LeftExpression = LeftExpression;
        this.RightExpression = RightExpression;
       
    }

    public override string Evaluate()
    {
        bool sol = (double.Parse(LeftExpression.Evaluate())) >= double.Parse((RightExpression.Evaluate()));
        return sol.ToString();
    }
}