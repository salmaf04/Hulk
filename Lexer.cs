class Lexer{
    private readonly string _text;
    private int position;
    public List<SyntaxToken> tokens;

    //Arreglo con palabras claves
    string[] KeyWord = {"let", "function", "in", "if", "else"};

    public Lexer(string text){
        _text = text;
        tokens = new List<SyntaxToken>();
        CleanToken();
    }

    void CleanToken(){
         SyntaxToken token;
        do{
            token = this.NextToken();

            if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken){
                tokens.Add(token);
            }
        } while (token.Kind != SyntaxKind.EndofFileToken);
        
    }

    // Funcion para seleccionar caracter que voy a analizar
    private char Current{
        get{
            if (position >= _text.Length){
                return '\0';
            }
        return _text[position];
        }
    }
    // Funcion para pasar a la siguiente posicion
    private void Next(){
        position++;
    }
    //Funcion para Tokenizar
    public SyntaxToken NextToken(){
       
        //Si la posicion que esta analizando, es igual o mayor que el tamaño del texto, 
        //se crea un SyntaxToken tipo EndOfFile
        if (position >= _text.Length){
            return new SyntaxToken(SyntaxKind.EndofFileToken, position, "\0", null);
        }
        //Si el caracter que esta analizando es un numero, analiza el siguiente, hasta que encuentre algo distinto a un numero
        if(char.IsDigit(Current)){
            //Guarda la posicion del primer caracter que encontro como numero
            var start = position;

            while(char.IsDigit(Current) || Current == '.')
                Next();
            //Guarda la posicion que termino, restandole donde empezo, quedando la longitud del numero
            var length = position - start;
            //Guarda un string con el numero que se obtiene
            var text = _text.Substring(start, length);
            //Guarda el valor del numero
            int.TryParse(text, out var value);
            //Se crea un SyntaxToken de tipo Number
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }
         /*Si el caracter que esta analizando es una letra, analiza el siguiente,
         hasta que encuentre algo distinto a una letra o numero*/
         if(char.IsLetter(Current)){
            //Guarda la posicion del primer caracter que encontro como letra
            var start = position;

            while(char.IsLetter(Current) || char.IsDigit(Current))
                Next();
            //Guarda la posicion que termino, restandole donde empezo, quedando la longitud del texto
            var length = position - start;
            //Guarda un string con el texto que se obtiene
            var text = _text.Substring(start, length);
           
            /*Analizar si el texto obtenido corresponde a una funcion predefinida,
            en caso de serlo se crea un SyntaxToken tipo PredefinedFunction*/
            if (PredefinedFunction.PredefinedFunctions.Contains(text)) {
                return new SyntaxToken(SyntaxKind.PredefinedFunctionToken, start, text, text);
            }
            //Analizar si el texto obtenido es una palabra clave, en caso de serlo se crea un SyntaxToken tipo KeyWord
            if (KeyWord.Contains(text)){
                return new SyntaxToken(SyntaxKind.KeyWordToken, start, text, text);
            }
            //Sino, se crea un SyntaxToken tipo Word
            return new SyntaxToken(SyntaxKind.WordToken, start, text, text);
        }
            /*Analizar strings dentro de '"':
            en caso del que el caracter que se este analizando sea una comilla*/
            if (Current == '"'){
                //Guardar la posicion deonde va a empezar el texto que se encuentra entre comillas
                 var start = position + 1;
                 Next();
            //Pasar a la siguiente posicion hasta que se encuentre una comilla, o llegue al ultimo caracter del texto
            while(Current != '"' && position != _text.Length - 1)
                Next();
            /*Si sale del while y el caracter en el que esta parado es distinto a una doble comilla,
            significa que esta nunca se cerro, por lo que lanza una excepcion de error de sintaxis*/
            if (Current != '"'){
                throw new SyntaxException("Missing a quote");
            }
            //Guarda el tamaño del texto que se encuentra entre '"'
            var length = position - start;
            //Guarda un string con todo el texto que se encuentra entre '"'
            var text = _text.Substring(start, length);
            Next();
            //Crea un SyntaxToken tipo String
            return new SyntaxToken(SyntaxKind.StringToken, start, text, text);
            }

        //Analizar si es un espacio
        if (char.IsWhiteSpace(Current)){
            //Guarda la posicion del primer caracter que encontro como espacio
             var start = position;
            //Mientras lo sea, pasa a la siguiente posicion
            while(char.IsWhiteSpace(Current))
                Next();
             //Guarda la posicion que termino, restandole donde empezo, quedando la longitud de los espacios
            var length = position - start;
            //Guarda un string con los espacios
            var text = _text.Substring(start, length);
            //Se crea un SyntaxToken tipo Whitespace
            return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
        }
        switch(Current){
            //Si el caracter es un '+' se crea un SyntaxToen tipo Plus
            case '+':
                return new SyntaxToken(SyntaxKind.PlusToken, position++, "+", null);
            //Si el caracter es un '-' se crea un SyntaxToen tipo Minus
            case '-':
                return new SyntaxToken(SyntaxKind.MinusToken, position++, "-", null);
            //Si el caracter es un '*' se crea un SyntaxToen tipo Asterik
            case '*':
                return new SyntaxToken(SyntaxKind.AsterikToken, position++, "*", null);
            //Si el caracter es un '/' se crea un SyntaxToen tipo Slash
            case '/':
                return new SyntaxToken(SyntaxKind.SlashToken, position++, "/", null);
            //Si el caracter es un '(' se crea un SyntaxToen tipo OpenParenthesis
            case '(':
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, position++, "(", null);
            //Si el caracter es un ')' se crea un SyntaxToen tipo CloseParenthesis
            case ')':
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, position++, ")", null);
            //Si el caracter es un '^' se crea un SyntaxToen tipo Caret
            case '^':
                return new SyntaxToken(SyntaxKind.CaretToken, position++, "^", null);
            //Si el caracter es un '=':
            case '=':
                //Pasar al siguiente caracter
                Next();
                //Si es un igual, entonces se crea un nuevo SyntaxToken tipo EqualCompare,
                
                if (Current == '=')
                    return new SyntaxToken(SyntaxKind.EqualCompareToken, position++, "==", null);
                //Si es un mayor que, entonces se crea un nuevo SyntaxToken tipo Arrow
                if (Current == '>')
                    return new SyntaxToken(SyntaxKind.ArrowToken, position++, "=>", null);
                //Sino se crea un nuevo SyntaxToken tipo Equal*/
                return new SyntaxToken(SyntaxKind.EqualToken, position, "=", null);
            //Si el caracter es un '!':
            case '!':
                //Pasar al siguiente caracter
                Next();
                 /*Si es un igual, entonces se crea un nuevo SyntaxToken tipo DifferentCompare,
                sino se crea un nuevo SyntaxToken tipo Negation*/
                if (Current == '=')
                    return new SyntaxToken(SyntaxKind.DifferentCompareToken, position++, "!=", null);
                return new SyntaxToken(SyntaxKind.NegationToken, position, "!", null);
            //Si el caracter es un '@' se crea un SyntaxToen tipo AtSign
            case '@':
                return new SyntaxToken(SyntaxKind.AtSignToken, position++, "@", null);
            case '%':
                return new SyntaxToken(SyntaxKind.PorcentToken, position++, "%", null);   
            //Si el caracter es un ',' se crea un SyntaxToen tipo Comma
            case ',':
                return new SyntaxToken(SyntaxKind.CommaToken, position++, ",", null);
            //Si el caracter es un ';' se crea un SyntaxToen tipo SemiColon
            case ';':
                return new SyntaxToken(SyntaxKind.SemiColonToken, position++, ";", null);
            //Si el caracter es un '&':
            case '&':
                //Pasar al siguiente caracter
                Next();
                /*Si es un '&', entonces se crea un nuevo SyntaxToken tipo And,
                sino se lanza una excepcion por error sintactico*/
                if (Current == '&'){
                    return new SyntaxToken(SyntaxKind.AndToken, position++, "&&", null);
                }
            throw new SyntaxException("Operator '&' does not exist");
            //Si el caracter es un '|':
            case '|':
                //Pasar al proximo caracter
                Next();
                /*Si es un '|', entonces se crea un nuevo SyntaxToken tipo Or,
                sino se lanza una excepcion por error sintactico*/
                if (Current == '|'){
                    return new SyntaxToken(SyntaxKind.OrToken, position++, "||", null);
                }
                 throw new SyntaxException("Operator '|' does not exist");
            //Si el caracter es un '<':
            case '<':
                //Pasar al siguiente caracter
                Next();
                /*Si es un igual, entonces se crea un nuevo SyntaxToken tipo MenorIgual,
                sino se crea un nuevo SyntaxToken tipo Menor*/
                if (Current == '=')
                    return new SyntaxToken(SyntaxKind.MenorIgualToken, position++, "<=", null);
                return new SyntaxToken(SyntaxKind.MenorToken, position, "<", null);
            //Si el caracter es un '>':
            case '>':
                //Pasar al siguiente caracter
                Next();
                 /*Si es un igual, entonces se crea un nuevo SyntaxToken tipo MayorIgual,
                sino se crea un nuevo SyntaxToken tipo Mayor*/
                if (Current == '=')
                    return new SyntaxToken(SyntaxKind.MayorIgualToken, position++, ">=", null);
                return new SyntaxToken(SyntaxKind.MayorToken, position, ">", null);
             /*Si no es ninguno de los anterioeres, es un caracter no reconocido y se crea un SyntaxToken 
            tipo BadToken*/
            default:
                return new SyntaxToken(SyntaxKind.BadToken, position++, _text.Substring(position - 1, 1), null);
            }
           
    }
    //Funcion para comprobar parentesis balanceados
    public static bool BalancedParentheses(string input){
        int count = 0;
        //Ciclo para recorrer el texto
        for (int i = 0; i < input.Length; i++){
           //Si en alguna posicion encuentra '(', suma 1 a count y pasa a la siguiente posicion
            if (input[i] == '('){
                count ++;
                continue;
            }
            //Si en alguna posicion encuentra ')', resta 1 a count
            if (input[i] == ')'){
                count --;
            }
            //Si count es menor que 0, entonces no estan balanceados
            if (count < 0)
            return false;
        }
        //Si al terminar el ciclo count es igual a 0, entonces estan balanceados
        if (count == 0)
            return true;
        //Si count no es igual a 0, no estan balanceados
        return false;
    }
     public static void CheckSemicolon (string text){
        if (text [text.Length - 1] != ';'){
            throw new SyntaxException("Missing semicolon at the end of the expression");
        }
    }
}