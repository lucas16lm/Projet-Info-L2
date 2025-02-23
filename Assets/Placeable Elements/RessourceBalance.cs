using System;

[System.Serializable]
public struct RessourceBalance
{
    public int gold;
    public int meleeWeapons;
    public int rangedWeapons;
    public int horses;
    public int wood;

    public bool RemoveRessources(int gold, int meleeWeapons, int rangedWeapons, int horses, int wood){
        if(gold > this.gold || meleeWeapons > this.meleeWeapons || rangedWeapons > this.rangedWeapons || horses > this.horses || wood > this.wood){
            return false;
        }
        this.gold-=gold;
        this.meleeWeapons-=meleeWeapons;
        this.rangedWeapons-=rangedWeapons;
        this.horses-=horses;
        this.wood-=wood;
        return true;
    }
    public bool RemoveRessources(RessourceBalance ressourceBalance){
        return RemoveRessources(ressourceBalance.gold, ressourceBalance.meleeWeapons, ressourceBalance.rangedWeapons, ressourceBalance.horses, ressourceBalance.wood);
    }

    public void AddRessources(int gold, int meleeWeapons, int rangedWeapons, int horses, int wood){
        this.gold+=gold;
        this.meleeWeapons+=meleeWeapons;
        this.rangedWeapons+=rangedWeapons;
        this.horses+=horses;
        this.wood+=wood;
    }
    public void AddRessources(RessourceBalance ressourceBalance){
        AddRessources(ressourceBalance.gold, ressourceBalance.meleeWeapons, ressourceBalance.rangedWeapons, ressourceBalance.horses, ressourceBalance.wood);
    }
}
