using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD.Net
{
    public class NetEnemy : NetCharacter
    {
        public int ID { get; private set; }
        public string Description { get; private set; }
        public string EnterCombatText { get; private set; }
        public string RoomEnterText { get; private set; }
        public NetDrops InstanceDrops { get; private set; }


        public static NetEnemy CreateEnemy(string name, string description,
            NetDrops drops,
            NetPassive[] passives, NetActive[] actives,
            int id,
            string enterCombatText = "", string roomEnterText = "") 
        {
            NetEnemy e = new NetEnemy();

            e.Name = name;
            e.Description = description;
            e.EnterCombatText = enterCombatText;
            e.RoomEnterText = roomEnterText;
            e.InstanceDrops = drops;

            e.type = CharacterType.ENEMY;

            e.Passives = passives;
            e.Actives = actives;

            e.ID = id;

            return e;
        }


        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            ID = e.Reader.ReadInt32();
            Description = e.Reader.ReadString();
            EnterCombatText = e.Reader.ReadString();
            RoomEnterText = e.Reader.ReadString();
            InstanceDrops = e.Reader.ReadSerializable<NetDrops>();
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(ID);
            e.Writer.Write(Description);
            e.Writer.Write(EnterCombatText);
            e.Writer.Write(RoomEnterText);
            e.Writer.Write(InstanceDrops);
        }
    }
}