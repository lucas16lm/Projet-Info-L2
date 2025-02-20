using System;

[System.Serializable]
public struct RessourceBalance
{
    public int gold;
    public int weapons;
    public int powder;
    public int horses;
    public int wood;

    public void AddRessources(int gold, int weapons, int powder, int horses, int wood){
        this.gold+=gold;
        this.weapons+=weapons;
        this.powder+=powder;
        this.horses+=horses;
        this.wood+=wood;
    }

    public void RemoveRessources(int gold, int weapons, int powder, int horses, int wood){
        AddRessources(-gold, -weapons, -powder, -horses, - wood);
    }
    public void RemoveRessources(RessourceBalance ressourceBalance){
        RemoveRessources(ressourceBalance.gold, ressourceBalance.weapons, ressourceBalance.powder, ressourceBalance.horses, ressourceBalance.wood);
    }
}
