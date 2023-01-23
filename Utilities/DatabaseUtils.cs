using LiteDB;
using Screener.Models;

namespace Screener.Utilities
{
    public static class DatabaseUtils
    {
        private static decimal AllPositionsPercent;
        private static List<PositionModel> AllOpenNowPositionsList;
        private static string Log;

        public static void SavePosition(List<PositionModel> currentPositions, string positionsCollectionName, string tradesCollectionName, decimal depo)
        {
            using (var db = new LiteDatabase(@"Filename = ../../../AllTradeInfo.db; connection = shared"))
            {
                AllOpenNowPositionsList = currentPositions;

                GetAllPositionsPercent(currentPositions, depo);

                // Get a collection (or create, if doesn't exist)
                var positionsCollection = db.GetCollection<PositionModel>(positionsCollectionName);
                var tradesCollection = db.GetCollection<TradeModel>(tradesCollectionName);

                var allPositionsInDb = positionsCollection.FindAll();

                foreach (var positionInDb in allPositionsInDb)
                {
                    var samePositionsName = currentPositions.FindAll(x => x.Name == positionInDb.Name);

                    if (samePositionsName.Count > 0)
                    {
                        foreach (var currentPosition in samePositionsName)
                        {
                            if (positionInDb.Direction == currentPosition.Direction)
                            {
                                if (positionInDb.Equals(currentPosition)) // есть в бд, позиции равны
                                {
                                    currentPositions.Remove(currentPosition);
                                    continue;
                                }

                                // позиция изменилась, вычисляем трейд, обновляем бд
                                Log = $"Position changed. Pos in db = {positionInDb}, current position = {currentPosition}";

                                TradeModel trade = GetTrade(positionInDb, currentPosition, depo, Log);
                                tradesCollection.Upsert(trade);

                                positionInDb.EntryPrice = currentPosition.EntryPrice;
                                positionInDb.VolumeCoin = currentPosition.VolumeCoin;
                                positionInDb.VolumeUSDT = currentPosition.VolumeUSDT;

                                positionsCollection.Upsert(positionInDb);
                                db.Commit();

                                currentPositions.Remove(currentPosition);
                            }
                        }
                    }
                    else // в бд есть поза, которой уже нет в текущих
                    {
                        Log = $"Position closed. Remove from db = {positionInDb}";

                        TradeModel trade = GetTrade(positionInDb, "Database", depo, Log);
                        tradesCollection.Upsert(trade);

                        positionsCollection.Delete(positionInDb.Id);
                        db.Commit();
                    }
                }

                if (currentPositions.Count > 0) // в текущих позициях остались те, которых нет в бд
                {
                    foreach (var pos in currentPositions)
                    {
                        Log = $"Position opened. Add to db = {pos}";

                        TradeModel trade = GetTrade(pos, "Current", depo, Log);
                        tradesCollection.Upsert(trade);

                        positionsCollection.Upsert(pos);
                    }

                    db.Commit();
                }
            }
        }

        private static void GetAllPositionsPercent(List<PositionModel>positions, decimal depo)
        {
            decimal volume = 0;

            foreach (var pos in positions)
            {
                volume += pos.VolumeUSDT;
            }

            AllPositionsPercent = Math.Round(volume * 100 / depo, 2);
        }

        private static TradeModel GetTrade(PositionModel position, string location, decimal depo, string comment)
        {
            TradeModel trade = new TradeModel();

            trade.Name = position.Name;

            if (position.Direction == "Short")
            {
                if (location == "Database")
                {
                    trade.Direction = "Long"; // закрыл
                }
                else
                {
                    trade.Direction = "Short"; // открыл
                }
            }
            else
            {
                if (location == "Database")
                {
                    trade.Direction = "Short"; // закрыл 
                }
                else
                {
                    trade.Direction = "Long"; // открыл
                }
            }

            trade.VolumeUSDT = position.VolumeUSDT;

            trade.PositionDepoPercent = GetDepoPercent(depo, trade.VolumeUSDT);

            trade.VolumeCoin = position.VolumeCoin;

            if (location == "Database")
            {
                // market
            }
            else
            {
                trade.Price = position.EntryPrice;
            }
            
            trade.DateTimeUTC0 = DateTime.UtcNow;

            trade.Depo = depo;

            trade.AllPositionsDepoPercent = AllPositionsPercent;

            trade.AllOpenNowPositions = AllOpenNowPositionsList;

            return trade;
        }


        private static TradeModel GetTrade(PositionModel positionInDb, PositionModel currentPosition, decimal depo, string comment)
        {
            TradeModel trade = new TradeModel();

            trade.Name = positionInDb.Name;


            if (positionInDb.Direction == "Short")
            {
                if (positionInDb.VolumeUSDT > currentPosition.VolumeUSDT)
                {
                    trade.Direction = "Long"; // закрыл часть
                }
                else
                {
                    trade.Direction = "Short"; // добавил
                }
            }
            else
            {
                if (positionInDb.VolumeUSDT > currentPosition.VolumeUSDT)
                {
                    trade.Direction = "Short"; // закрыл часть
                }
                else
                {
                    trade.Direction = "Long"; // добавил
                }
            }

            trade.VolumeUSDT = Math.Abs(positionInDb.VolumeUSDT - currentPosition.VolumeUSDT);

            trade.PositionDepoPercent = GetDepoPercent(depo, trade.VolumeUSDT);

            trade.VolumeCoin = Math.Abs(positionInDb.VolumeCoin - currentPosition.VolumeCoin);

            trade.DateTimeUTC0 = DateTime.UtcNow;

            trade.Depo = depo;

            trade.AllPositionsDepoPercent = AllPositionsPercent;

            trade.AllOpenNowPositions = AllOpenNowPositionsList;

            return trade;
        }

        private static decimal GetDepoPercent(decimal depo, decimal positionValue)
        {
            return Math.Round(positionValue * 100 / depo, 2);
        }
    }
}
