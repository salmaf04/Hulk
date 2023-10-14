class SemanticException : Exception{
   public SemanticException(string message): base("! SEMANTIC ERROR: " + message ){
    }
}