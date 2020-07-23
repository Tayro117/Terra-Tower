using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(UseItem u, FoundRecipes cm) {
    	BinaryFormatter formatter = new BinaryFormatter();
    	string path = Application.persistentDataPath + "/player.inv";
    	FileStream stream = new FileStream(path, FileMode.Create);
    	PlayerData pd = new PlayerData(u, cm);
    	formatter.Serialize(stream, pd);
    	stream.Close();
    }

    public static PlayerData LoadPlayer() {
    	string path = Application.persistentDataPath + "/player.inv";
    	if (File.Exists(path)) {
    		BinaryFormatter formatter = new BinaryFormatter();
    		FileStream stream = new FileStream(path, FileMode.Open);
    		PlayerData d = formatter.Deserialize(stream) as PlayerData;
    		stream.Close();
    		return d;
    	}
    	else {
    		Debug.Log("Save file not found");
    		return null;
    	}
    }
    public static void SaveGrid(GridSystem g1, GridSystem g2) {
    	BinaryFormatter formatter = new BinaryFormatter();
    	string path = Application.persistentDataPath + "/grid.inv";
    	FileStream stream = new FileStream(path, FileMode.Create);
    	GridData gd = new GridData(g1, g2);
    	formatter.Serialize(stream, gd);
    	stream.Close();
        string chest_path = Application.persistentDataPath + "/temp_chests";
        foreach (string file in System.IO.Directory.GetFiles(chest_path))
            File.Copy(file, file.Replace("temp_chests","chest_data"), true);
    }
    public static GridData LoadGrid() {

        //Take care of chest loading
        var folder1 = Directory.CreateDirectory(Application.persistentDataPath + "/temp_chests");
        var folder2 = Directory.CreateDirectory(Application.persistentDataPath + "/chest_data");
        foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath + "/temp_chests"))
            File.Delete(file);
        string chest_path = Application.persistentDataPath + "/chest_data";
        foreach (string file in System.IO.Directory.GetFiles(chest_path))
            File.Copy(file, file.Replace("chest_data","temp_chests"), true);


    	string path = Application.persistentDataPath + "/grid.inv";
    	if (File.Exists(path)) {
    		BinaryFormatter formatter = new BinaryFormatter();
    		FileStream stream = new FileStream(path, FileMode.Open);
    		GridData d = formatter.Deserialize(stream) as GridData;
    		stream.Close();
    		return d;
    	}
    	else {
    		Debug.Log("Save file not found");
    		return null;
    	}
    }
    public static void SaveChest(GameObject obj, string name) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/temp_chests/chest_"+name+".inv";
        FileStream stream = new FileStream(path, FileMode.Create);
        ChestData cd = new ChestData(obj);
        formatter.Serialize(stream, cd);
        stream.Close();
    }
    public static ChestData LoadChest(string name) {
        string path = Application.persistentDataPath + "/temp_chests/chest_"+name+".inv";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ChestData d = formatter.Deserialize(stream) as ChestData;
            stream.Close();
            return d;
        }
        else {
            Debug.Log("Save file not found");
            return null;
        }
    }
}
