class Bool : Expression  {
    
    public Bool (string value){
        this.value = value;
    }
    public override string value {get;}
    public override SyntaxKind Kind {get;}
    public override string Evaluate(){
        return value;
    }
}