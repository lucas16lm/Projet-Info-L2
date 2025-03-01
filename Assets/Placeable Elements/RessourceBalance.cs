using System;

[System.Serializable]
public struct RessourceBalance
{
    public int gold;

    public bool RemoveRessources(int gold){
        if(gold > this.gold){
            return false;
        }
        this.gold-=gold;
        return true;
    }
    public bool RemoveRessources(RessourceBalance ressourceBalance){
        return RemoveRessources(ressourceBalance.gold);
    }

    public void AddRessources(int gold){
        this.gold+=gold;
    }
    public void AddRessources(RessourceBalance ressourceBalance){
        AddRessources(ressourceBalance.gold);
    }
}
