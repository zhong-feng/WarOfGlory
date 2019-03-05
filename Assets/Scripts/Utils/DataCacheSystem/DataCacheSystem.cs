using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.IO;

public class DataCacheSystem
{

    #region 单例模板代码
    private static DataCacheSystem _instance;
    public static DataCacheSystem Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new DataCacheSystem();
            }

            return _instance;
        }
    }

    protected DataCacheSystem()
    {
        //预留接口
        OnInitSingleton();
    }


    #endregion

    public Dictionary<MonsterType, MonsterPropModel> monsterDic = new Dictionary<MonsterType, MonsterPropModel>();

    public Dictionary<SkillID, SkillProp> skillDic = new Dictionary<SkillID, SkillProp>();

    public Dictionary<ItemID, ItemProp> itemDic = new Dictionary<ItemID, ItemProp>();

    string directoryPath = null;   //文件夹的路径
    string monsterFileName = null;   // monster 文件名
    string skillFileName = null;     // skill  文件名
    string itemFileName = null;     // Item 文件名

    public virtual void OnInitSingleton()
    {
        directoryPath = Application.dataPath + @"/Resources/Configs";//文件夹的路径
        monsterFileName = "Monster.xml";   //monster文件名字
        skillFileName = "Skill.xml";       //skill  文件名字
        itemFileName = "Item.xml";     // skill  文件名

        loadMonsterXml();  //本地 XML加载 monster 信息
        loadSkillXml();    //本地 XML加载 skill   信息
        loadItemrXml();    //本地 XML加载 item   信息
    }

    #region  加载怪物信息(XML)
    //加载怪物信息(XML)
    void loadMonsterXml()
    {
        //确认路径
        string path = directoryPath + "//" + monsterFileName;

        //先确认文件是否存在
        if (File.Exists(path))
        {
            //解析 Xml，添加字典
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList list = xmlDoc.FirstChild.SelectNodes("Monster");

            foreach (XmlElement element in list)
            {
                MonsterPropModel go = new MonsterPropModel();

                go.monsterType=(MonsterType)int.Parse( element.GetAttribute("type"));
                go.name = element.GetAttribute("name");
                go.modelPath = element.GetAttribute("modelPath");
                go.curHP=go.maxHP= int.Parse(element.GetAttribute("HP"));
                go.curMP=go.maxMP= int.Parse(element.GetAttribute("MP"));
                go.attact = float.Parse(element.GetAttribute("attact"));
                go.defense = float.Parse(element.GetAttribute("defense"));
                go.attackRadius = float.Parse(element.GetAttribute("attackRadius"));
                go.giveEXP = int.Parse(element.GetAttribute("exp"));
                go.giveMoney = int.Parse(element.GetAttribute("money"));

                monsterDic[go.monsterType] = go;
            }

        }
        else  //文件不存在，手动创建
        {
            Debug.LogError("monster is missing");
        }
    }
    #endregion
    
    #region  加载技能信息(XML)
    //加载怪物信息(XML)
    void loadSkillXml()
    {
        //确认路径
        string path = directoryPath + "//" + skillFileName;

        //先确认文件是否存在
        if (File.Exists(path))
        {
            //解析 Xml，添加字典
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList list = xmlDoc.FirstChild.SelectNodes("Skill");

            foreach (XmlElement element in list)
            {
                SkillProp go = new SkillProp();

                go.ID = (SkillID)int.Parse(element.GetAttribute("id"));
                go.skillName = element.GetAttribute("name");
                go.iconPath = element.GetAttribute("iconPath");
                go.skillCD = int.Parse(element.GetAttribute("skillCD"));
                go.use = int.Parse(element.GetAttribute("use"));
                go.propRation = float.Parse(element.GetAttribute("propRation"));
                go.radiusRatio = float.Parse(element.GetAttribute("radiusRatio"));
                go.describe = element.GetAttribute("describe");

                skillDic[go.ID] = go;
            }

        }
        else  //文件不存在，报错丢失文件
        {
            Debug.LogError("monster is missing");
        }
    }
    #endregion
    
    #region  加载物品信息(XML)
    //加载怪物信息(XML)
    void loadItemrXml()
    {
        //确认路径
        string path = directoryPath + "//" + itemFileName;

        //先确认文件是否存在
        if (File.Exists(path))
        {
            //解析 Xml，添加字典
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList list = xmlDoc.FirstChild.SelectNodes("Item");

            foreach (XmlElement element in list)
            {
                //先给基类属性赋值
                ItemProp itemPropGo = new ItemProp();

                itemPropGo.id = (ItemID)(int.Parse(element.GetAttribute("id")));   //注意
                itemPropGo.type = (ItemType)(int.Parse(element.GetAttribute("type")));

                itemPropGo.name = element.GetAttribute("name");
                itemPropGo.desc = element.GetAttribute("desc");
                
                itemPropGo.buyPrice = int.Parse(element.GetAttribute("buyPrice"));
                itemPropGo.sellPrice = int.Parse(element.GetAttribute("sellPrice"));
                
                itemPropGo.maxNum = int.Parse(element.GetAttribute("maxNum"));

                itemPropGo.iconPath = element.GetAttribute("iconPath");

                //再给派生类属性赋值
                if(itemPropGo .type == ItemType.IT_Drug)
                {
                    DrugProp go = new DrugProp(itemPropGo);

                    go.hp = int.Parse(element.GetAttribute("hp"));
                    go.mp = int.Parse(element.GetAttribute("mp"));

                    itemPropGo = go;
                }
                else if(itemPropGo.type == ItemType.IT_Weapon)
                {
                    WeaponProp go = new WeaponProp(itemPropGo);

                    go.attack = int.Parse(element.GetAttribute("atk"));

                    itemPropGo = go;
                }
                else if(itemPropGo.type == ItemType.IT_Armor)
                {
                    ArmorProp go = new ArmorProp(itemPropGo);

                    go.defense = int.Parse(element.GetAttribute("def"));

                    itemPropGo = go;
                }

                itemDic[itemPropGo.id] = itemPropGo;
            }

        }
        else  //文件不存在，手动创建
        {
            Debug.LogError("monster is missing");
        }
    }
    #endregion

    //获取怪物信息
    public MonsterPropModel GetMonsterInfo(MonsterType _type)
    {
        if(monsterDic.ContainsKey(_type))
        {
            //先给基类属性赋值
            MonsterPropModel go = new MonsterPropModel(monsterDic[_type]);

            //再给派生类属性赋值
            if((int)_type < (int)MonsterType.MT_LastHero)
            {
                HeroPropModel hero = new HeroPropModel(go);

                return hero;
            }

            return go;
        }

        return null;
    }

    //获取技能信息
    public SkillProp GetSkillInfo(SkillID _id)
    {
        if (skillDic.ContainsKey(_id))
        {
            SkillProp go = new SkillProp(skillDic[_id]);
            return go;
        }

        return null;
    }

    //获取物品信息
    public ItemProp GetItemProp(ItemID _id)
    {
        if(itemDic.ContainsKey(_id))
        {
            //if(itemDic[_id].type == ItemType.IT_Drug)
            //{
            //    DrugProp go = new DrugProp(itemDic[_id]);
            //    return go;
            //}
            //else if (itemDic[_id].type == ItemType.IT_Weapon)
            //{
            //    WeaponProp go = new WeaponProp(itemDic[_id]);
            //    return go;
            //}
            //else if (itemDic[_id].type == ItemType.IT_Armor)
            //{
            //    ArmorProp go = new ArmorProp(itemDic[_id]);
            //    return go;
            //}

            return itemDic[_id].Clone();
        }

        return null;
    }
}
