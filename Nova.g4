grammar Nova;

// Keywords (this is not a production rule)
//KEYWORDS:-
//  PACKAGE
//  MODULE
//  LET
//  I16
    
expr
    : let
    ;

container: (module|expr);

// Constructs
package
    : PACKAGE ID LBRA (module NEWLINE*)* RBRA
    ;

module
    : MODULE ID LBRA (container NEWLINE*)* RBRA
    ;
    
let
    : LET ID EQ 
        (
            LBRA 
            NEWLINE* 
                (expr (NEWLINE*|SEMI?))* 
                (literal|binary)
            NEWLINE* 
            RBRA 
            | 
            (literal|binary) (NEWLINE*|SEMI?)
        )
    ;

literal
    : I16
    ;

binary
    : addition
    ;

addition
    : multiplication ( (PLUS | MINUS) multiplication )*
    ;

multiplication
    : atom ( (MULT | DIV) atom )*
    ;


//// Combined binary operations
//binary
//    : atom
//    | addition
//    | subtraction
//    | multiplication
//    | division
//    ;
//
//// Define arithmetic operations
//addition
//    : atom PLUS binary
//    ;
//
//subtraction
//    : atom MINUS binary
//    ;
//
//multiplication
//    : atom MULT binary
//    ;
//
//division
//    : atom DIV binary
//    ;

// Atomic values (base units)
atom
    : I16
    | LPAR binary RPAR
    ;
    
// Keywords (reserved words)
PACKAGE : 'package';
MODULE  : 'module';
LET     : 'let';

// Types
I16     : [0-9]+;

// Identifiers
ID      : [a-zA-Z_][a-zA-Z0-9_]*;

// Punctuation
PLUS    : '+';
MINUS   : '-';
MULT    : '*';
DIV     : '/';
MOD     : '%';
LPAR    : '(';
RPAR    : ')';
COLON   : ':';
SEMI   : ';';
LBRA    : '{';
RBRA    : '}';
EQ      : '=';
LT      : '<';
GT      : '>';
LTE     : '<=';
GTE     : '>=';

// Handling newlines and skipping them
NEWLINE : '\r'? '\n' -> channel(HIDDEN);

// Handling whitespace and putting it to the hidden channel
WS      : [ \t]+ -> channel(HIDDEN);