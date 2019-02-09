using System;
using System.Threading.Tasks;
using StarMarines.Models;
using Newtonsoft.Json;
using System.Linq;

namespace StarMarines.Strategies
{
    //Первая конкретная реализация-стратегия 
    public class BasicStrategy : AbstratctStrategy
    {
        public override Command OnReceived(Screen message)
        {
            var rand = new Random();
            Command command = new Command(); // формируем команду
            if (message.planets.Count() > 0) {  // проверяем наличие планет в ответе
                var my = message.planets.Where(x=> { // выираем только свои планеты
                    if(x.owner != null){
                        return x.owner.Equals(BotName,StringComparison.InvariantCultureIgnoreCase);
                    }
                    return false;                    
                }).FirstOrDefault(); // берем первую
                if (my != null){ // проверяем а остались ли свои планеты?
                    var nei = my.neighbours.ToList();  // находим соседей
                    var next_p = nei.Skip(rand.Next(nei.Count-1)).FirstOrDefault(); // выбираем случайного соседа
                    command.actions = new Models.Action[1] { new Models.Action{ // формируем действие. Можно сформироват ьпо одному действию с каждой вашей планеты.
                        from = my.id, 
                        to = next_p, 
                        unitsCount = rand.Next(1, my.droids)
                        } 
                    };
                }
            }
            if (message.errors.Count() > 0) // а есть ли ошибки?
            {
                Console.WriteLine(JsonConvert.SerializeObject(message.errors)); // увы есть.. покажем их.
            }
            return command;
        }
    }
}