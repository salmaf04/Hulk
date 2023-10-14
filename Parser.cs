class Parser{
    private readonly List <SyntaxToken> _tokens;
    private int _position;
    public Expression tree;
    public Scope scope;
    public Parser (List<SyntaxToken> tokens, Scope? scope = null){
        if (scope == null){
            scope = new Scope();
        }
        this.scope = scope;
       _tokens = tokens;
        tree = ParserLevel1();
    }
    /*Funcion que retorna el token que se encuentra en la posicion que se esta analizando 
    mas el valor del parametro que se le pasa*/
    private SyntaxToken Peek (int offset){
        int index = _position + offset;
        if (index >= _tokens.Count){
            return _tokens [_tokens.Count - 1];
        }
        return _tokens[index];
    }
    private SyntaxToken Current => Peek(0);
//Funcion para pasar al siguiente Token de la lista
    private SyntaxToken NextToken(){
        SyntaxToken current = Current;
        _position++;
        return current;
    }
    //Analizar suma o resta
    Expression ParserLevel1(){
        Expression left = ParserLevel2();
        while (Current.Kind == SyntaxKind.PlusToken || Current.Kind == SyntaxKind.MinusToken){
            //Guardar si es una suma o una resta 
            SyntaxKind operador = Current.Kind;
            NextToken();
            Expression right = ParserLevel2();
            if (operador == SyntaxKind.PlusToken) 
               left = new Addition (left, right);
            else
            left = new Sustraction (left, right);
        }
        return left;
        
    }
    //Analizar multiplicacion, division o resto
    Expression ParserLevel2(){
        Expression left = ParserLevel3();
        while (Current.Kind == SyntaxKind.AsterikToken || Current.Kind == SyntaxKind.SlashToken || Current.Kind == SyntaxKind.PorcentToken){
            //Guardar si es una multiplicacion, una division o un resto
            SyntaxKind operador = Current.Kind;
            NextToken();
            Expression right = ParserLevel3();
            if (operador == SyntaxKind.AsterikToken){ 
                left = new Product (left, right);
            }
            else if (operador == SyntaxKind.SlashToken){
                left = new Division (left, right);
            }
            else {
                left = new Rest (left, right);
            }
        }
        return left;
    }
    //Analizar si es una potencia
    Expression ParserLevel3(){
        Expression left = Concat();
        while (Current.Kind == SyntaxKind.CaretToken){
            NextToken();
            Expression right = Concat();
            left = new Power(left, right);
        }
        return left;     
    }
    //Analizar si es una concatenacion
    Expression Concat(){
        Expression left = And();
        while (Current.Kind == SyntaxKind.AtSignToken){
            NextToken();
            Expression right = And();
            left = new Concat(left, right);
        }
        return left; 
    }
    //Analizar operador and (&&)
    Expression And(){
        Expression left = Or();
        while (Current.Kind == SyntaxKind.AndToken){
            NextToken();
            Expression right = Or();
            left = new AndOperator(left, right);
        }
        return left; 
    }
    //Analizar operador or (||)
    Expression Or(){
        Expression left = CompareEqual();
        while (Current.Kind == SyntaxKind.OrToken){
            NextToken();
            Expression right = CompareEqual();
            left = new OrOperator(left, right);
        }
        return left; 
    }
    //Analizar operador igual que (==)
    Expression CompareEqual(){
        Expression left = CompareDifferent();
        while (Current.Kind == SyntaxKind.EqualCompareToken){
            NextToken();
            Expression right = CompareDifferent();
            left = new EqualCompare(left, right);
        }
        return left; 
    }
    //Analizar operador distinto que (!=)
    Expression CompareDifferent(){
        Expression left = MenorIgual();
        while (Current.Kind == SyntaxKind.DifferentCompareToken){
            NextToken();
            Expression right = MenorIgual();
            left = new DifferentCompare(left, right);
        }
        return left; 
    }
    //Analizar operador menor igual que (<=)
    Expression MenorIgual(){
        Expression left = Menor();
        while (Current.Kind == SyntaxKind.MenorIgualToken){
            NextToken();
            Expression right = Menor();
            left = new MenorIgualOperator(left, right);
        }
        return left; 
    }
    //Analizar operador menor que (<)
    Expression Menor(){
        Expression left = MayorIgual();
        while (Current.Kind == SyntaxKind.MenorToken){
            NextToken();
            Expression right = MayorIgual();
            left = new MenorOperator(left, right);
        }
        return left; 
    }
    //Analizar operador mayor igual que (>=)
    Expression MayorIgual(){
        Expression left = Mayor();
        while (Current.Kind == SyntaxKind.MayorIgualToken){
            NextToken();
            Expression right = Mayor();
            left = new MayorIgualOperator(left, right);
        }
        return left; 
    }
    //Analizar operador mayor igual que (>=)
    Expression Mayor(){
        Expression left = ParserLevel4();
        while (Current.Kind == SyntaxKind.MayorToken){
            NextToken();
            Expression right = ParserLevel4();
            left = new MayorOperator(left, right);
        }
        return left; 
    }
    
     Expression ParserLevel4(){
        switch(Current.Kind){
            //En caso que sea un numero
            case SyntaxKind.NumberToken:
                //Pasar a la siguiente posicion
                NextToken();
                //Retorna el SyntaxToken de la posicion anterior a la que esta parada
                return new Number(Peek(-1).Text);
            //En caso que sea un string
            case SyntaxKind.StringToken:
            NextToken();
            //Retorna el SyntaxToken de la posicion anterior a la que esta parada
            return new Text(Peek(-1).Text);
            //Si es un parentesis abierto llama a la funcion parentesis
            case SyntaxKind.OpenParenthesisToken:
                return Parentheses();;
            //Si encuentra una funcion predefinida
            case SyntaxKind.PredefinedFunctionToken:
                //Guarda el nombre de la funcion
                string operador = Current.Text;
                /*Si el proximo SyntaxToken de la lista no es un parentesis abierto, se lanza 
                una excepcion por error sintactico*/
                if (Peek(1).Kind != SyntaxKind.OpenParenthesisToken)
                    throw new SyntaxException ("Open Parenthesis Expected after " + Current.Text);
                return new PredefinedFunction(operador, GetParams());
            /*Si encuentra un menos, pasa al siguiente SyntaxToken, y se crea un numero que es el opuesto 
            del valor que se obtiene al evaluar en ParserLevel4*/ 
            case SyntaxKind.MinusToken:
                NextToken();
                return new Number((-double.Parse(ParserLevel4().Evaluate())).ToString());
            /*Caso de encontrar un SyntaxToken tipo WordToken, se crea una variable, que sera el texto del
            Token, en caso de estar en la lista de nombres de variables*/
            case SyntaxKind.NegationToken:
                NextToken();
                return new Bool ((!bool.Parse(ParserLevel4().Evaluate())).ToString());
            case SyntaxKind.WordToken :
                Variable? v = scope.FindVar(Current.Text);
                //Si no es nula, se pasa al siguiente SyntaxToken y se retorna el valor de la variable
                if (v != null){
                    NextToken();
                    return v.value;
                }
                Function? f = scope.FindFun(Current.Text);
                //Si no es nula, se pasa al siguiente SyntaxToken y se retorna el valor de la variable
                if (f != null){
                    if (Peek(1).Kind == SyntaxKind.OpenParenthesisToken){
                        List<Expression> paramsValue = GetParams();
                        if (f.parametros.Count == paramsValue.Count){
                            for (int i = 0; i < paramsValue.Count; i++){
                                string name = f.parametros[i];
                                Expression value = paramsValue [i];
                                scope.variables.Add(new Variable(name, value));
                            }
                            return new Parser(f.fun, scope).tree;
                        }
                        throw new SemanticException ("Los parametros no son correctos");
                    }
                }
                //Sino se lanza una excepcion por error sintactico
                throw new SyntaxException("Variable not found");
        }
        //Si se encuentra la palabra clave let, llama a la funcion SolveLet
        if (Current.Text == "let"){
            NextToken();
            return SolveLet();
        }
        //Si se encuentra la palabra clave function, llama a la funcion SolveFunction
        if (Current.Text == "function"){
            NextToken();
            return SolveFunction();
        }
        //Si se encuentra la palabra clave if, llama a la funcion Conditional
        if (Current.Text == "if"){
            NextToken();
            return Conditional();
        }

        throw new LexicalException("Invalid Token");
    }

    Expression Parentheses(){
        List<SyntaxToken> t = new List<SyntaxToken>();
        /* Se crea una variable para llevar cuenta de parentesis, empieza en uno
        por el parentesis abierto por el que se llamo a la funcion */
        int count = 1;
        NextToken();
        /*Mientras que count sea distinta de cero, se añade el SyntaxToken a la lista 
        y se pasa al siguiente*/
        while(count != 0){
           
            //Si el SyntaxToken es un parentesis abierto se le suma uno a count
            if (Current.Kind == SyntaxKind.OpenParenthesisToken){
                count ++;
            }
            //Si el SyntaxToken es un parentesis cerrado se le resta uno a count
            if (Current.Kind == SyntaxKind.CloseParenthesisToken){
                count --;
            }  
            if (count == 0){
                break;
            }
            t.Add(Current);
            NextToken();
        } 
        NextToken();
        
        return new Parser (t, scope).tree;
    }
    List<Expression> GetParams(){
        List <Expression> paparams = new List<Expression>();
        List<SyntaxToken> totokens = new List<SyntaxToken>();
        int count = 1;
        NextToken();
        while(count != 0){
            NextToken();
            if (Current.Kind == SyntaxKind.OpenParenthesisToken){
                count ++;
            }
            if (Current.Kind == SyntaxKind.CloseParenthesisToken){
                count --;
            }
            if(count == 0)
                break;
            if (Current.Kind == SyntaxKind.CommaToken && count == 1){
                paparams.Add(new Parser(totokens, scope).tree);
                totokens.Clear();
                continue;
            }
            totokens.Add(Current);
            
        } 
        NextToken();
        paparams.Add(new Parser(totokens, scope).tree);
        return paparams;
    }
    Expression SolveLet(){
        string name;
        Expression value;
        //Si el SyntaxToken es tipo WordToken, el texto se guarda como nombre de la variable
        if (Current.Kind == SyntaxKind.WordToken){
            name = Current.Text;
            NextToken();
            if (Current.Kind == SyntaxKind.EqualToken){
                NextToken();
                List<SyntaxToken> l = new List<SyntaxToken>();
                /*Mientras el SyntaxToken sea distinto de la palabra clave in y de una coma se añadira
                el SyntaxToken a la lista l y se pasara al siguiente SyntaxToken */
                while (Current.Text != "in" && Current.Kind != SyntaxKind.CommaToken){
                    l.Add(Current);
                    NextToken();
                    /*Si la posicion es igual al tamaño de la lista de SyntaxTokens de la clase Parser
                    se lanza una excepcion por error de Sintaxis  */
                    if (_position == _tokens.Count){
                        throw new SyntaxException ("Missing in KeyWord");
                    }
                }
                //Se le da el valor a la variable
                value = new Parser (l, scope).tree;
                //Si sale por una coma, debe ser porque se declara otra variable por lo que se vuelve a llamar a SolveLet
                if (Current.Kind == SyntaxKind.CommaToken){
                    NextToken();
                    //se añade la variable a la lista de la clase Variable
                    scope.variables.Add(new Variable(name,value));
                    return SolveLet();
                }
                //En otro caso salio por el in, por lo que se analiza lo que se encuentra detras
                NextToken();
                List<SyntaxToken> l2 = new List<SyntaxToken>();
                //Mientras sea distinta de un punto y coma, se añade el SyntaxToken y se pasa al siguiente 
                while (Current.Kind != SyntaxKind.SemiColonToken){
                    l2.Add(Current);
                    NextToken();
                }
                //Se añade el punto y coma a la lista
                l2.Add(Current);
                //se añade la variable a la lista de la clase Variable
                scope.variables.Add(new Variable(name,value));
                //Se retorna la lista de lo que se encuentra despues del in 
                return new Parser (l2, scope).tree;
            }
        }
        throw new SyntaxException("Bad variable declaration");
    }
    
    Expression Conditional(){
        /*Se evalua lo que esta dentro del parentesis, si devuelve true, se hace una lista de SyntaxToken,
        hasta que encuente un else, y se devuelve esta lista */
        if (Parentheses().Evaluate() == "True"){
            List<SyntaxToken> l = new List<SyntaxToken>();
            while (Current.Text != "else"){
                l.Add(Current);
                NextToken();
            }
            return new Parser (l, scope).tree;
        }
        /*En caso de no ser true, se recorre hasta el else, y se hace una lista con los SyntaxToken 
        despues del else hasta llegar a la ultima posicion*/
        while (Current.Text != "else"){
            NextToken();
        }
        NextToken();
        List<SyntaxToken> l2 = new List<SyntaxToken>(); 
        while (_position < _tokens.Count){
            l2.Add(Current);
            NextToken();
        }
         return new Parser (l2, scope).tree;
    }

    Expression SolveFunction(){
        string name;
        //Si el SyntaxToken es tipo WordToken, el texto se guarda como nombre de la funcion
        if (Current.Kind == SyntaxKind.WordToken){
            name = Current.Text;
            NextToken();
            if (Current.Kind == SyntaxKind.OpenParenthesisToken){
                NextToken();
                List<string> param = GetParamsFromFun();
                if (Current.Kind == SyntaxKind.ArrowToken){
                    NextToken();
                    List <SyntaxToken> fun = new List<SyntaxToken>();
                    while (Current.Kind != SyntaxKind.SemiColonToken){
                    fun.Add(Current);
                    NextToken();
                    }
                    //Se añade el punto y coma a la lista
                    fun.Add(Current);
                    //Se añade la funcion a la lista e string de la clase Function
                    Scope funScope = new Scope();
                    funScope.functions.Add(new Function(name,param, fun));
                    Console.Write(">");
                    string input = Console.ReadLine();
                    Lexer.BalancedParentheses(input);
                    Lexer.CheckSemicolon(input.TrimEnd());
                    List <SyntaxToken> tokens = new Lexer(input).tokens;
                    return  new Parser(tokens, funScope).tree;
                }
            }
        }  
        throw new SyntaxException("Bad function declaration");
    }
    List <string> GetParamsFromFun(){
        List <string> param = new List<string> ();
        while (Current.Kind != SyntaxKind.CloseParenthesisToken){
            if (Current.Kind != SyntaxKind.CommaToken){
            param.Add(Current.Text);
            }
            NextToken();
        }
        NextToken();
        return param;
    }
}