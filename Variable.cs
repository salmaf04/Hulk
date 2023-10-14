class Variable{
    public string name {get;}
    public Expression value {get;}
    public Variable(string name, Expression value){
        this.name = name;
        this.value = value;
    }
}