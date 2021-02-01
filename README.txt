How to Run:
1. Open Program.fs file on VSC
2. Open Terminal
3. Select all code or CTRL + A and press Alt + Enter
4. Go to line 138 and press Alt + Enter
5. At line 139 press Alt + Enter to be given a result

Make a new variable to interpret:
1. Start with "let variable_name ="
2. Use any expression of either:
  - IntConstant
  - BooleanConstant
  - BinaryOp
  - ComparisonExpression
  - IfExpression
  - LetExpression
  - Variable Expression
Note: read the eval function to see the arguments. FunctionExpression is currently not added in.

Example:
let pSample = BinaryOp(
  PLUS,
  400,
  74
)

Then in main after the env variable, insert pSample after eval and before env.
