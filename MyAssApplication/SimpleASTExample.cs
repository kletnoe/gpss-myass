using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using MyAssCompiler;

namespace MyAssApplication
{
    public class SimpleASTExample
    {
        //public ASTModel GetAst()
        //{
            //IdsList.It.Ids.Add(-1, "STORAGE");
            //IdsList.It.Ids.Add(-2, "INITIAL");
            //IdsList.It.Ids.Add(-3, "GENERATE");
            //IdsList.It.Ids.Add(-4, "SAVEVALUE");
            //IdsList.It.Ids.Add(-5, "TEST");
            //IdsList.It.Ids.Add(-6, "QUEUE");
            //IdsList.It.Ids.Add(-7, "ENTER");
            //IdsList.It.Ids.Add(-8, "DEPART");
            //IdsList.It.Ids.Add(-9, "ADVANCE");
            //IdsList.It.Ids.Add(-10, "LEAVE");
            //IdsList.It.Ids.Add(-11, "TERMINATE");
            //IdsList.It.Ids.Add(-12, "X");

            //IdsList.It.Ids.Add(10000, "Server");
            //IdsList.It.Ids.Add(10001, "RejectCounter");
            //IdsList.It.Ids.Add(10002, "GenerateCounter");
            //IdsList.It.Ids.Add(10003, "RejetionProb");

            //IdsList.It.Ids.Add(20000, "Exponential");



        //    ASTModel model = new ASTModel();

        //    // STORAGE
        //    model.Verbs.Add(new ASTBlock()
        //    {
        //        LabelId = 10000,
        //        VerbId = -1,
        //        Operands = new ASTOperands()
        //        {
        //            Operands = new List<ASTOperand>()
        //            {
        //                new ASTLiteral()
        //                {
        //                    LiteralType = LiteralType.Int32,
        //                    Value = 3
        //                }
        //            }
        //        }
        //    });
                    
        //     // INITIAL 1
        //    model.Verbs.Add(new ASTBlock()
        //    {
        //        LabelId = int.MinValue,
        //        VerbId = -2,
        //        Operands = new ASTOperands()
        //        {
        //            Operands = new List<ASTOperand>()
        //            {
        //                new ASTLValue()
        //                {
        //                    Id = -12,
        //                    Accessor = new ASTDirectSNA()
        //                    {
        //                        Id = 10001
        //                    }
        //                },
        //                new ASTLiteral()
        //                {
        //                    LiteralType = LiteralType.Int32,
        //                    Value = 0
        //                }
        //            }
        //        }
        //    });

        //    // INITIAL 2
        //    model.Verbs.Add(new ASTBlock()
        //    {
        //        LabelId = int.MinValue,
        //        VerbId = -2,
        //        Operands = new ASTOperands()
        //        {
        //            Operands = new List<ASTOperand>()
        //            {
        //                new ASTLValue()
        //                {
        //                    Id = -12,
        //                    Accessor = new ASTDirectSNA()
        //                    {
        //                        Id = 10002
        //                    }
        //                },
        //                new ASTLiteral()
        //                {
        //                    LiteralType = LiteralType.Int32,
        //                    Value = 0
        //                }
        //            }
        //        }
        //    });

        //    // INITIAL 3
        //    model.Verbs.Add(new ASTBlock()
        //    {
        //        LabelId = int.MinValue,
        //        VerbId = -2,
        //        Operands = new ASTOperands()
        //        {
        //            Operands = new List<ASTOperand>()
        //            {
        //                new ASTLValue()
        //                {
        //                    Id = -12,
        //                    Accessor = new ASTDirectSNA()
        //                    {
        //                        Id = 10003
        //                    }
        //                },
        //                new ASTLiteral()
        //                {
        //                    LiteralType = LiteralType.Int32,
        //                    Value = 0
        //                }
        //            }
        //        }
        //    });

        //    // INITIAL 3
        //    model.Verbs.Add(new ASTBlock()
        //    {
        //        LabelId = int.MinValue,
        //        VerbId = -2,
        //        Operands = new ASTOperands()
        //        {
        //            Operands = new List<ASTOperand>()
        //            {
        //                new ASTLValue()
        //                {
        //                    Id = -12,
        //                    Accessor = new ASTDirectSNA()
        //                    {
        //                        Id = 10003
        //                    }
        //                },
        //                new ASTLiteral()
        //                {
        //                    LiteralType = LiteralType.Int32,
        //                    Value = 0
        //                }
        //            }
        //        }
        //    });

        //    // GENERATE
        //    model.Verbs.Add(new ASTBlock()
        //    {
        //        LabelId = int.MinValue,
        //        VerbId = -3,
        //        Operands = new ASTOperands()
        //        {
        //            Operands = new List<ASTOperand>()
        //            {
        //                new ASTLValue()
        //                {
        //                    Id = 20000,
        //                    Accessor = new ASTCall()
        //                    {
        //                        Actuals = new ASTActuals()
        //                        {
        //                            //Expressions = new List<ASTExpr>()
        //                            //{
        //                            //    new ASTLiteral()
        //                            //    {
        //                            //        LiteralType = LiteralType.Int32,
        //                            //        Value = 1
        //                            //    },
        //                            //    new ASTLiteral()
        //                            //    {
        //                            //        LiteralType = LiteralType.Int32,
        //                            //        Value = 0
        //                            //    },
        //                            //    new ASTBinaryExpr()
        //                            //    {
        //                            //        LValue = new ASTLiteral()
        //                            //        {
        //                            //            LiteralType = LiteralType.Int32,
        //                            //            Value = 1
        //                            //        },
        //                            //        RValue = new ASTLiteral()
        //                            //        {
        //                            //            LiteralType = LiteralType.Int32,
        //                            //            Value = 2
        //                            //        },
        //                            //        Operator = BinaryOperatorType.DIVIDE
        //                            //    }
        //                            //}
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    });

        //    return model;
        //}
    }
}
