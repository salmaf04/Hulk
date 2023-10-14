class Scope{
    public List<Variable> variables;
    public List<Function> functions;

    public Scope (){
        functions = new List<Function>();
        variables = new List<Variable>();
        variables.Add(new Variable("Pi", new Number (Math.PI.ToString())));
        variables.Add(new Variable("E", new Number (Math.E.ToString())));
    }
    public  Variable? FindVar (string name){
        foreach (Variable v in variables){
            if (name == v.name){
                return v;
            }        
        }
        return null; 
    }
    public  Function? FindFun (string name){
        foreach (Function f in functions){
            if (name == f.name){
                return f;
            }        
        }
        return null; 
    }
}