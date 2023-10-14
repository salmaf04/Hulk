class SyntaxException : Exception{
    public SyntaxException(string message): base("! SYNTAX ERROR: " + message ){
    }
}