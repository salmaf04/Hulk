 abstract class Expression {
    public abstract SyntaxKind Kind {get;}
    public abstract string value{get;}

   public abstract string Evaluate();
}