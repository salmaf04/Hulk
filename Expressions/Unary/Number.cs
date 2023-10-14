class Number : Expression  {
    
    public Number (string value){
        this.value = value;
    }
    public override string value {get;}
    public override SyntaxKind Kind {get;}
    public override string Evaluate(){
        return value;
    }
}