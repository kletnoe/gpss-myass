using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples.NakedTestModels
{
    public static class Model_Lusin
    {
        public static string Model
        {
            get
            {
                return @"
@using MyAss.Framework.BuiltIn.Blocks
@using MyAss.Framework.BuiltIn.Commands

@usingp MyAss.Framework.BuiltIn.SNA.StorageSNA

STO STORAGE 4 ;места под автостоянку
COL STORAGE 2 ;бензоколонки
; описание работы бензоколонки
GENERATE 4 ;приезд автомобиля
TEST E SF$STO,0,BYBY ;если места заняты - проезжает
ENTER STO ;занять место на автостоянке
ENTER COL ;занять бензоколонку
LEAVE STO ;освободить автостоянку
ADVANCE 5 ;заправиться
LEAVE COL ;освободить бензоколонку
BYBY TERMINATE ;покинуть станцию
;таймер
GENERATE 720
TERMINATE 1
START 100

";
            }
        }
    }
}
