class Function{
    public string name;
    public List<string> parametros;
    public List <SyntaxToken> fun;

    public Function(string name, List<string> parametros, List<SyntaxToken> fun){
        this.name = name;
        this.parametros = parametros;
        this.fun = fun;
    }


}