// Amy Ngo
// CS 474 - Assignment 6
open System

type Operator = PLUS | MINUS | TIMES | DIV

type Comparator = EQUAL

type Value =
    | IntValue of int32
    | BoolValue of bool

type Name (nm:string)=
    member this.TheName = nm 

type Expression =
    | IntConstant of int32
    | BooleanConstant of bool
    | BinaryOp of Operator * Expression * Expression
    | ComparisonExpression of Comparator * Expression * Expression
    | IfExpression of Expression * Expression * Expression
    | LetExpression of Name * Expression * Expression
    | VariableExpression of Name

// Binding class
type Binding (name:Name, value:Value) =
    member this.BName = name
    member this.Value = value

// make Environment class
type Environment(bindings: Binding list) =
    // lookup variable in list
    let rec lookupRec (variable:Name) (bs:Binding list) =
        match bs with
        | [] -> failwithf "Name %A not found in environment" variable
        | b::_ when b.BName.TheName.Equals(variable.TheName) -> b.Value
        | _::tl -> lookupRec variable tl

    // call bind and look up method in class type
    member public y.lookup variable = lookupRec variable bindings
    // Dynamic bind function
    member public this.bind n v = 
        let b = Binding (name = n, value = v)
        let newList = b::bindings
        Environment(newList)

// https://stackoverflow.com/questions/47335279/fsharp-passing-n-parameters-for-a-function

// =========== evaluate function ===========
// c - expression, e - environment
let rec eval c (e:Environment) =
    match c with
    | IntConstant (value) -> (IntValue value)
    | BooleanConstant (bool) -> (BoolValue bool)
    | BinaryOp (op, left, right) ->
        try
            let (IntValue l) = eval left e
            let (IntValue r) = eval right e
            match op with
            | PLUS -> IntValue (l + r)
            | MINUS -> IntValue (l - r)
            | TIMES -> IntValue (l * r)
            | DIV when r = 0 -> failwithf "Can not divide by 0"
            | DIV -> IntValue (l / r)
            | _ -> failwithf "Unknown binary operator: %A" op
        with
            | _ -> failwithf "Binary operation error"
    | ComparisonExpression (comp, left, right) ->
        let (IntValue l) = eval left e
        let (IntValue r) = eval right e
        match comp with
        | EQUAL -> BoolValue (l = r)
        | _ -> failwithf "Unknown comparison type: %A" comp
    | IfExpression (cond, thenSide, elseSide) ->
        let (BoolValue b) = eval cond e
        match b with
        | true -> eval thenSide e
        | false -> eval elseSide e
    | LetExpression (variable, value, body) -> 
        // let eLet = new Environment()
        let v1 = eval value e
        let newE = e.bind variable v1
        // evaluate body with new environment
        eval body newE
    | VariableExpression variable ->
        // look up variable with the list of binding name and expression
        e.lookup variable
    | _ -> failwithf "Unknown expression: %A" c

// p sample problem - Expected 237

let p1 = LetExpression(
    Name("bot"),
    IntConstant(3),
    LetExpression(
        Name("bot"),
        IntConstant(2),
        IfExpression(
            ComparisonExpression(
                EQUAL,
                VariableExpression(Name("bot")),
                IntConstant(0)),
            BinaryOp(
                DIV, 
                IntConstant(474), 
                IntConstant(0)
            ),
            BinaryOp(
                DIV, 
                BinaryOp(
                    PLUS, 
                    IntConstant(400), 
                    IntConstant(74)
                ), 
                VariableExpression(Name("bot"))
            )
        )
    )
)

// =========== main function ===========
[<EntryPoint>]
let main argv =
    let env = Environment([])
    printfn "p1 Value: %A" (eval p1 env)
    0 // return an integer exit code