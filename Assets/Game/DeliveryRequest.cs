using System.Linq;
using UnityEngine;

namespace Game
{
    public class DeliveryRequest
    {
        [System.Serializable]
        public struct Request
        {
            public Goods.GoodsType goodsType;
            public int need;
            public int satisfied;
        }

        public Request[] requests;
        public float deadline;

        public void Provide(Goods.GoodsType goodsType)
        {
            requests = requests.Select(request =>
            {
                if (request.goodsType == goodsType)
                {
                    if (request.satisfied < request.need)
                    {
                        return new Request
                        {
                            goodsType = request.goodsType,
                            need = request.need,
                            satisfied = request.satisfied + 1
                        };
                    }
                }

                return request;
            }).ToArray();
        }

        public bool Fulfilling(Goods.GoodsType goodsType)
        {
            return requests.Any(request =>
            {
                if (request.goodsType == goodsType)
                {
                    if (request.satisfied < request.need)
                    {
                        return true;
                    }
                }

                return false;
            });
        }
    }
}