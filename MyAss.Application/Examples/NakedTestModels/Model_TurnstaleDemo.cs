using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples.NakedTestModels
{
    public static class Model_TurnstaleDemo
    {
        public static string Model
        {
            get
            {
                return @"
@using MyAss.Framework_v2.BuiltIn.Blocks
@using MyAss.Framework_v2.BuiltIn.Commands

Server STORAGE 1	;Инициализация турникета
START 10000		;Параметры  запуска модели

GENERATE 7,5	;Люди прибывают
QUEUE Turn		;Вхождение в очередь
ENTER Server	;Начало использования турникета
DEPART Turn		;Отбывание из очереди
ADVANCE 5,3		;Задержка использования турникета
LEAVE Server	;Конец использования турникета
TERMINATE 1		;Один человек проходит

";
            }
        }
    }
}
