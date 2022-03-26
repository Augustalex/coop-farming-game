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

        public void Provide(Goods goods)
        {
            requests = requests.Select(request =>
            {
                if (request.goodsType == goods.goodsType)
                {
                    if (request.satisfied < request.need)
                    {
                        // WARNING, SIDE EFFECTS
                        goods.Consume();

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
    }
}