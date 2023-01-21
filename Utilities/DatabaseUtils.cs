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
                var col = db.GetCollection<PositionModel>(positionsCollectionName);

                var allPositionsInDb = col.FindAll();

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

                                positionInDb.EntryPrice = currentPosition.EntryPrice;
                                positionInDb.VolumeCoin = currentPosition.VolumeCoin;
                                positionInDb.VolumeUSDT = currentPosition.VolumeUSDT;

                                col.Upsert(positionInDb);
                                db.Commit();

                                currentPositions.Remove(currentPosition);
                            }
                        }
                    }
                    else // в бд есть поза, которой уже нет в текущих
                    {
                        // TODO вычисление и запись в трейды, удаление из бд

                        col.Delete(positionInDb.Id);
                        db.Commit();
                    }
                }

                if (currentPositions.Count > 0) // в текущих позициях остались те, которых нет в бд
                {
                    // TODO вычисление и запись в трейды, добавление в бд

                    foreach (var pos in currentPositions)
                    {
                        col.Upsert(pos);
                    }
                    
                    db.Commit();
                }
            }
        }
    }
}
