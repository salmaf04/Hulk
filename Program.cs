using System;
namespace Program{
    class Program{
       static List<SyntaxToken>  tokens = new List<SyntaxToken>();
     
        public static void Main(string[] args){

            while (true){
                
                try{
                    Console.Write(">");
                    string input = Console.ReadLine();
                    Lexer.BalancedParentheses(input);
                    Lexer.CheckSemicolon(input.TrimEnd());
                    List <SyntaxToken> tokens = new Lexer(input).tokens;
                    Parser p = new Parser(tokens);
                    System.Console.WriteLine(p.tree.Evaluate());
           

               
                } catch(LexicalException e){
                    System.Console.WriteLine(e.Message);
                    
                }catch(SyntaxException s){
                    System.Console.WriteLine(s.Message);
                    
                }
            }
        }
        
        bool CheckSyntax(){
           if (tokens[tokens.FindIndex(t=>t.Kind == SyntaxKind.NumberToken) + 1].Kind == SyntaxKind.WordToken){
            System.Console.WriteLine("! LEXICAL ERROR:");
           }
            return false;
        }
    }
}
