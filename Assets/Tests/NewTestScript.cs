using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
    public class NewTestScript
    {


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestPlayerCtrl()
        {
            PlayerCtrl x = new PlayerCtrl();
            Assert.AreEqual(1, x.bulletIDnumPlayer);
            //Assert.AreEqual(2, x.bulletIDnumPlayer);
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
        [UnityTest]
        public IEnumerator TestEnemyCtrl()
        {
            EnemyCtrl x = new EnemyCtrl();
            Assert.AreEqual(Vector3.zero, x.GetEnemyBulletDirection());
            //Assert.AreEqual(2, x.bulletIDnumPlayer);
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
