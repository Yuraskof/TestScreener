using LiteDB;
using Screener.Models;

namespace Screener.Utilities
{
    public static class DatabaseUtils
    {
        public static void SavePosition(List<PositionModel> currentPositions, string positionsCollectionName, string tradesCollectionName)
        {
            using (var db = new LiteDatabase(@"Filename = ../../../AllTradeInfo.db; connection = shared"))
            {
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

                                // TODO вычисление и запись в трейды
                                TradeModel trade = GetTrade(positionInDb, currentPosition);
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
                        // TODO вычисление и запись в трейды, удаление из бд

                        TradeModel trade = GetTrade(positionInDb, "Database");
                        tradesCollection.Upsert(trade);

                        positionsCollection.Delete(positionInDb.Id);
                        db.Commit();
                    }
                }

                if (currentPositions.Count > 0) // в текущих позициях остались те, которых нет в бд
                {
                    // TODO вычисление и запись в трейды, добавление в бд

                    foreach (var pos in currentPositions)
                    {
                        TradeModel trade = GetTrade(pos, "Current");
                        tradesCollection.Upsert(trade);

                        positionsCollection.Upsert(pos);
                    }

                    db.Commit();
                }
            }
        }

        private static TradeModel GetTrade(PositionModel position, string location)
        {
            TradeModel trade = new TradeModel();

            trade.Name = position.Name;

            if (position.Direction == "Short")
            {
                if (location == "Database")
                {
                    trade.Name = "Long"; // закрыл
                }
                else
                {
                    trade.Name = "Short"; // открыл
                }
            }
            else
            {
                if (location == "Database")
                {
                    trade.Name = "Short"; // закрыл 
                }
                else
                {
                    trade.Name = "Long"; // открыл
                }
            }

            trade.VolumeUSDT = position.VolumeUSDT;

            trade.VolumeCoin = position.VolumeCoin;

            trade.Price = position.EntryPrice;

            trade.DateTimeUTC0 = DateTime.UtcNow;

            return trade;
        }


        private static TradeModel GetTrade(PositionModel positionInDb, PositionModel currentPosition)
        {
            TradeModel trade = new TradeModel();

            trade.Name = positionInDb.Name;


            if (positionInDb.Direction == "Short")
            {
                if (positionInDb.VolumeUSDT > currentPosition.VolumeUSDT)
                {
                    trade.Name = "Long"; // закрыл часть
                }
                else
                {
                    trade.Name = "Short"; // добавил
                }
            }
            else
            {
                if (positionInDb.VolumeUSDT > currentPosition.VolumeUSDT)
                {
                    trade.Name = "Short"; // закрыл часть
                }
                else
                {
                    trade.Name = "Long"; // добавил
                }
            }

            trade.VolumeUSDT = Math.Abs(positionInDb.VolumeUSDT - currentPosition.VolumeUSDT);

            trade.VolumeCoin = Math.Abs(positionInDb.VolumeCoin - currentPosition.VolumeCoin);

            trade.DateTimeUTC0 = DateTime.UtcNow;

            return trade;
        }
    }
}
