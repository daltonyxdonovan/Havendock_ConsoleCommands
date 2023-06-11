using BepInEx;
using BepInEx.Unity;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace cheatBox;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]

public class Plugin : BaseUnityPlugin
{
    
    bool isInGame = false;
    Vector3 playerPos;
    int ticker = 0;
    int difficultTicker = 0;
    bool everActivated = false;
    string input = "";
    public TMP_InputField cheatBox;
    public Canvas canvas;
    public GameObject chat;
    bool difficult = false;
    int difficulty = 1;
    bool godmode = false;
    int godmodeTicker = 0;
    public Cheats cheats;
    public TextMeshProUGUI command_text;
    string[] textToDisplay;
    bool gathering = false;
    string[] items = new string[] { "True","true","False","false","Vegetables", "Wood", "People", "Water", "Happiness", "Charcoal", "Seed", "Seawater", "Teriyaki", "Fish", "Hunger", "Golden_Retriever", "Steel", "Meat", "Purple_Melon", "Sand", "Cooked_Vegetables", "Cooked_Fish", "Cooked_Meat", "Golden_Fish", "Iron_Ore", "Penguin_Poop", "Brick", "Star", "Glass", "Oxygen_Tank", "Alcohol", "Berry", "Egg", "Fruit_Punch", "Power", "Minerals", "Raw_Minerals", "Bot", "Circuit_Board", "Blueprint_Piece", "Herb", "Bacon", "White_Sand", "Pasta", "Recipe_Book", "Feather", "Medicine", "Broken_Radio", "Fishing_Rod", "Hammer", "Knowledge", "Radio", "Health", "Copper_Ore", "Bronze", "Wheat", "Flour", "Biscuit", "Sugarcane", "Salmon", "Chicken_Feed", "Corn", "Hay", "Chick", "Chicken", "Mealworm", "Crude_Oil", "Gear", "Coal", "Graphite", "Refined_Oil", "Plastic", "Wet_Seaweed", "Sushi", "Spore", "Dried_Seaweed", "Rope", "Plankton", "Ice", "Hook", "Oil_Extract", "Cooking_Oil", "Motor", "Battery", "Sleep", "Sulfur", "Dextrin", "Coffee_Bean", "Coffee", "Cocoa_Pod", "Chocolate", "Roasted_Coffee_Bean", "Tea", "Hot_Cocoa", "Pixie_Dust", "Moon_Dust", "Frog_Egg", "Pixie", "Crooked_Wand", "Basic_Wand", "Bone", "Potion_Lv1", "Potion_Lv2", "Magic_Broom", "Magic_Sauce", "Cotton", "Canvas", "Balloon", "Helium", "Nylon", "Coin", "Mysterious_Gem", "Crowbar", "Wooden_Shield", "Sandal", "Glove", "Steel_Shield", "Fishing_Rod_Lv2", "Cooking_Gloves", "Carrot", "Rabbit", "Thruster", "Book", "Drumstick", "Potion_Lv3", "Coconut", "Milk", "Cheese", "Tuna", "Peacock", "Orb", "Engine_Part", "Cow", "Grass", "Biofertilizer", "Palm_Leaf", "Thirst", "Brown_Meteorite", "Blue_Meteorite", "Red_Meteorite", "Strange_Device", "Brown_Cosmic_Dust", "Blue_Cosmic_Dust", "Red_Cosmic_Dust", "Quark", "Intermediate_Wand", "Mouse", "Salt", "Pearl", "Shell", "Donut", "Cheesecake", "Purple_Pie", "Ice_Cream", "Wet_Shirt", "Dry_Shirt", "Plastic_Bull", "Flotsam", "Trowel_Lv1", "Tiny_Top_Hat", "Tiny_Chef_Hat", "Tiny_Socks", "Slime_Bubble", "Slime_Pudding", "Slime_Sword", "Slime_Armor", "Skull_Key", "Magic_Sword", "Casein_Plastic" };
    string[] first_commands = new string[] { "pollution","infinitePower","pickupRange","clear","spawnSailorBoat", "spawnSettler", "spawnZooey", "scale", "spawnWitch", "spawnSteelTrader", "spawnTrader", "spawnPenguin", "flight", "enableContent", "unlockAllResearch", "researchSpeed", "trashSpawnInterval", "netMax", "easyGather", "fireDamage", "easyGather", "help", "respawn", "giveAll", "give", "difficulty", "godmode", "noFire", "yesFire", "buildBoatyard", "restoreAll", "spawnChickenBoat", "spawnMouse" };
    bool toggled_once = false;
    int gathertick = 0;
    bool doesFireDmg = false;
    bool fireadded = false;
    TrashSpawner trashSpawner;
    float trashSpawnRate = 4;
    bool trashSpawning = false;
    int trashSpawnTicker = 0;
    Image image;
    bool help = false;
    GameObject myPanel;
    float vertSpeed = 0.1f;
    Color transparent = new Color(0,0,0,0);
    Color nontransparent = new Color(0,0,0,0.9f);
    double version = 1.80;
    bool isPollutionEnabled = true;
    bool infPower = false;


    GameObject suggestions;
    bool flight = false;
    GameObject player;


    public void spawnTrash()
    {
        trashSpawner.SpawnSpecificTrash("Planks");
        
        //trashSpawner.SpawnSpecificTrash("GoldenFish");
        //trashSpawner.SpawnSpecificTrash("BigFish");
        trashSpawner.SpawnSpecificTrash("Crate");
        trashSpawner.SpawnSpecificTrash("PalmLeaf");
        trashSpawner.SpawnSpecificTrash("Fish");
    }

    public void Awake()
    {
        Logger.LogInfo($"Plugin ConsoleCommands v{version} is loaded!");
        
        //myPanel.SetActive(false);
    }

    public void Log(string message)
    {
        MessageScript.instance.displayMessageDirectly(message);
        Logger.LogInfo(message);
    }

    public void Update()
    {
        //we don't wanna call this every frame, so we'll use a ticker
        if (ticker > 0)
            ticker--;

        if (SceneManager.GetActiveScene().name.ToString() == "GameScene")
        {
            /*BuildingManager buildingManager = Toolbox.instance.buildingManager;
            BuildingData[] buildings = buildingManager.buildingDatas;

            for (int i = 0; i < buildings.Length; i++)
            {
                if (buildings[i].name == "Crane")
                {
                    if (!(buildings[i] is BuildingData))
                    {
                        continue;
                    }

                    BuildingData buildingData = buildings[i] as BuildingData;
                    if (buildingData != null)
                    {

                        CustomBuildingData customBuildingData = new CustomBuildingData
                        {
                            // Copy properties from buildingData to customBuildingData

                            name = buildingData.name
                        };

                        buildings[i] = (BuildingData)customBuildingData; // Explicit cast to BuildingData

                    }
                }
            }*/
        }

        
        


        if (!isPollutionEnabled)
        {
            Game.current.pollution = 0;
        }
        if (difficult)
        {
            difficultTicker++;
            if (difficultTicker > 30)
            {
                if (difficulty < 0)
                {
                    Log("Too low a difficulty! setting value to 0");
                    difficulty = 0;
                }
                Monster[] monsters = FindObjectsOfType<Monster>();
                if (monsters.Length > 0)
                {
                    foreach (Monster monster in monsters)
                    {
                        if (monster.gameObject.name.StartsWith("ThirdPersonController"))
                        {

                        }
                        else
                        {
                            monster.maxHP = difficulty;
                            //monster.hpBar.maxValue = difficulty;
                            //Log($"setting hp of {monster.gameObject.name} to {difficulty} and damage to {(difficulty > 0 ? difficulty / 4 : 0)}");
                            if (difficulty > 0)
                                monster.damage = difficulty / 4;
                            else if (difficulty == 0)
                                monster.damage = 0;
                        }
                        
                    }
                }
                
                difficultTicker = 0;
            }
            
        }
        if (infPower)
        {
            Game.current.power = 999f;
        }
        if (gathering)
        {
            gathertick++;
            if (gathertick > 60)
            {
                Trash[] trash = FindObjectsOfType<Trash>();

                if (GameObject.FindWithTag("Player") != null && !isInGame)
                {
                    //find gameobject tagged 'Player'
                    playerPos = player.transform.position;
                    Debug.Log(playerPos + " is the pos of player");
                    Logger.LogInfo("Player found?");
                    isInGame = true;
                }

                if (isInGame)
                {

                    foreach (Trash t in trash)
                    {
                        if (t.name.StartsWith("Planks"))
                        {
                            //move towards player
                            t.transform.position = Vector3.MoveTowards(t.transform.position, playerPos, 2f);
                            //Debug.Log("moving trash");


                        }
                        else if (t.name.StartsWith("Fish"))
                        {
                            //move towards player
                            t.transform.position = Vector3.MoveTowards(t.transform.position, playerPos, 3f);
                            //Debug.Log("moving trash");

                        }
                        else if (t.name.StartsWith("Crate"))
                        {
                            //move towards player
                            t.transform.position = Vector3.MoveTowards(t.transform.position, playerPos, 2f);
                            //Debug.Log("moving trash");

                        }
                        else if (t.name.StartsWith("PalmLeaf"))
                        {
                            //move towards player
                            t.transform.position = Vector3.MoveTowards(t.transform.position, playerPos, 2f);
                            //Debug.Log("moving trash");

                        }
                    }
                    //Debug.Log(sum_trash + " trash moved towards player");
                }
                gathertick = 0;
            }
            
        }
        if (godmode)
        {
            godmodeTicker++;
            if (godmodeTicker > 30)
            {
                foreach (var character in FindObjectsOfType<Character>())
                {
                    character.hunger = 100;
                    character.thirst = 100;
                    //Logger.LogInfo($"hunger and thirst set to max!");
                }
                godmodeTicker = 0;
                difficulty = 0;

                
            }
            difficultTicker++;
            if (difficultTicker > 30)
            {


                if (difficulty < 0)
                {
                    Log("Too low a difficulty! setting value to 0");
                    difficulty = 0;
                }
                Monster[] monsters = FindObjectsOfType<Monster>();
                if (monsters.Length > 0)
                {
                    foreach (Monster monster in monsters)
                    {
                        if (monster.gameObject.name.StartsWith("ThirdPersonController"))
                        {

                        }
                        else
                        {
                            monster.maxHP = difficulty;
                            monster.damage = difficulty;
                        }
                        // Log("setting difficulty to " + difficulty);
                    }
                }
                
                difficultTicker = 0;
            }
            
        }
        if (doesFireDmg)
        {

            //REALLY don't recommend using this, it will cause massive amounts of errors. gonna send a log to user


            //Log("doesFireDmg triggered");
            foreach (Deck deck in BuildingManager.instance.decks)
            {
                Log($"count: {BuildingManager.instance.decks.Count()}");
                if (!fireadded)
                {

                    //add fireScript to buildingScript
                    deck.gameObject.AddComponent<fireScript>();
                    //Log("fireScript added");
                }
                else
                {
                    //Log("MADE IT WOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                    if (deck.buildingScript.isOnFire)
                    {
                        //damage the health in firescript

                        deck.GetComponent<fireScript>().doDamage(deck.GetComponent<fireScript>().getDamage());
                        if (deck.GetComponent<fireScript>().getHealth() < 1)
                        {
                            BuildingManager.instance.ForcefullyDeleteBuildingLocally(deck.buildingScript);
                            Destroy(deck.GetComponent<fireScript>());
                            Log("You lost a deck!");
                        }
                    }
                }
            }




            foreach (BuildingScript buildingScript in BuildingManager.instance.buildings)
            {
                if (!fireadded)
                {
                    buildingScript.gameObject.AddComponent<fireScript>();
                    //Log($"count :: {BuildingManager.instance.buildings.Count}");
                }
                else
                {
                    if (buildingScript.isOnFire)
                    {
                        //damage the health in firescript
                        fireScript fs = buildingScript.gameObject.GetComponent<fireScript>();
                        fs.doDamage(fs.getDamage());
                        if (fs.getHealth() < 1)
                        {
                            //if name is not "crane"

                            Destroy(buildingScript);
                        }
                    }
                }
            }
            fireadded = true;
        }
        if (trashSpawning)
        {
            if (trashSpawner == null)
            {
                trashSpawner = FindObjectOfType<TrashSpawner>();

            }
            else
            {

                trashSpawnTicker++;
                if (trashSpawnTicker > 100 * trashSpawnRate)
                {
                    spawnTrash();
                    trashSpawnTicker = 0;
                }

            }

        }
        if (flight)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + vertSpeed, player.transform.position.z);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - vertSpeed, player.transform.position.z);
            }
            // Get the vertical and horizontal input axes
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            // Get the camera's forward and right vectors
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // Calculate the movement direction relative to the camera's orientation
            Vector3 movementDirection = (cameraForward * verticalInput) + (cameraRight * horizontalInput);

            // Set the y component to 0 to restrict movement to the x-z plane
            movementDirection.y = 0f;

            // Normalize the vector to ensure consistent movement speed
            movementDirection.Normalize();

            // Move the player in the calculated direction
            player.transform.Translate(movementDirection * (vertSpeed * 40) * Time.fixedDeltaTime, Space.World);
        }


        //arrows are to be used for the sandbox sliders for worldgen in main menu
        //can't use the textbox or a command because there _is_ no textbox to use a command here,
        //and I hate debugging making new objects
        if (Input.GetKeyDown(KeyCode.UpArrow) && SceneManager.GetActiveScene().name.ToString() != "GameScene")
        {
            Log("up" + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == "MainMenuScene")
            {
                SandboxSlider[] gameObjects = FindObjectsOfType<SandboxSlider>();

                for (int i = 0; i < gameObjects.Length; i++)
                {
                    if (gameObjects[i].multiplier != 1)
                        gameObjects[i].multiplier += 10;
                    else if (gameObjects[i].multiplier == 1)
                        gameObjects[i].multiplier += 9;
                }
                Log($"Multiplier is set to {gameObjects[0].multiplier}");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && SceneManager.GetActiveScene().name.ToString() != "GameScene")
        {
            Log("down" + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == "MainMenuScene")
            {
                SandboxSlider[] gameObjects = FindObjectsOfType<SandboxSlider>();
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    if (gameObjects[i].multiplier > 10)
                        gameObjects[i].multiplier -= 10;
                    else if (gameObjects[i].multiplier < 1)
                        gameObjects[i].multiplier = 1;
                    else if (gameObjects[i].multiplier == 10)
                        gameObjects[i].multiplier = 1;
                }
                Log($"Multiplier is set to {gameObjects[0].multiplier}");
            }
        }
        //slash enables the chatbox and activates it (as long as there isn't a tutorial box open, god forgid that happens lol)
        if (Input.GetKey(KeyCode.Slash) && SceneManager.GetActiveScene().name.ToString() == "GameScene")
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
                
            }
            /*BuildingManager buildingManager1 = Toolbox.instance.buildingManager;
            BuildingData[] buildings = buildingManager1.buildingDatas;
            for (int i = 0; i < buildings.Length; i++)
            {
                Log(buildings[i].name);
            }*/
            

            toggled_once = true;
            if (ticker == 0)
            {
                if (!everActivated)
                {
                    
                    //Harmony.CreateAndPatchAll(typeof(PatchBuildLimit));
                    //canvas is object named '~Canvas'
                    canvas = GameObject.Find("Canvas~").GetComponent<Canvas>();
                    myPanel = new GameObject("daltonyx_panel");

                    Debug.Log($"{canvas.name} has been created properly!");
                    
                    // Add the Image component to the panel GameObject
                    image = myPanel.AddComponent<UnityEngine.UI.Image>();
                    image.color = nontransparent;

                    

                    // Add the RectTransform component to the panel GameObject
                    RectTransform rectTransform2 = myPanel.GetComponent<RectTransform>();
                    rectTransform2.anchorMin = new Vector2(0f, 0f);
                    rectTransform2.anchorMax = new Vector2(1f, 1f);
                    rectTransform2.pivot = new Vector2(0.5f, 0.5f);
                    rectTransform2.sizeDelta = new Vector2(300f, 200f);

                    //set the parent
                    myPanel.transform.SetParent(canvas.transform, false);

                    


                    suggestions = new GameObject("suggestions");
                    suggestions.transform.SetParent(canvas.transform, false);

                    

                    chat = canvas.transform.Find("Chat").gameObject;
                    Debug.Log($"{chat.name} has been created properly!");
                    

                    //set myPanel to the child(4)
                    myPanel.transform.SetSiblingIndex(4);


                    cheatBox = chat.transform.Find("ChatInputField (TMP)").GetComponent<TMP_InputField>();
                    if (!toggled_once)
                        Log($"DevCommands are enabled! <color=red><3</color> <color=#62a4de>daltonyx</color>");
                    //cheatBox.gameObject.SetActive(false);
                    cheats = FindObjectOfType<Cheats>();
                    //add TextMeshProUGUI component to canvas.
                    command_text = suggestions.gameObject.AddComponent<TextMeshProUGUI>();
                    command_text.color = Color.white;
                    command_text.fontSize = 20;

                    // Add the RectTransform component to the text GameObject
                    RectTransform textRectTransform = command_text.GetComponent<RectTransform>();
                    textRectTransform.anchorMin = new Vector2(0f, 0f);
                    textRectTransform.anchorMax = new Vector2(1f, 1f);
                    textRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    textRectTransform.sizeDelta = new Vector2(0f, 0f);
                    textRectTransform.anchoredPosition = new Vector2(0f, 0f);
                    suggestions.transform.SetSiblingIndex(5);
                    //lower command_text by 50

                    command_text.alignment = TextAlignmentOptions.MidlineLeft;

                    textRectTransform.anchoredPosition = new Vector2(30, -100);
                }
                else
                {
                    if (command_text != null)
                    {


                        command_text.alignment = TextAlignmentOptions.MidlineLeft;
                        command_text.fontSize = 20;
                        RectTransform textRectTransform = command_text.GetComponent<RectTransform>();
                        textRectTransform.anchoredPosition = new Vector2(30, -100);
                        // command_text.margin = new Vector4(20, 170, 0, 0);
                    }
                }
                help = false;


                cheatBox.gameObject.SetActive(!cheatBox.gameObject.activeSelf);
                if (cheatBox.gameObject.activeSelf)
                {
                    //set alpha to 0
                    image.color = nontransparent;
                    cheatBox.text = "";
                    cheatBox.Select();
                    cheatBox.ActivateInputField();
                    everActivated = true;
                }
                else
                {

                    image.color = transparent;
                    command_text.text = "";
                    Destroy(myPanel);
                    Destroy(suggestions);

                    everActivated = false;
                }
                ticker = 10;
                //Logger.LogInfo($"isShowing: {isShowing}");
                

                
                
            }
            
        }

        if (Input.GetKey(KeyCode.Slash) && SceneManager.GetActiveScene().name.ToString() == "CaveScene")

        {
            Log("Use the difficult or godmode command <i>outside</i> the cave, there is no\ntextbox for me to use in the cave scene");
        }
            //everActivated & the scene check are the guard clauses for the main menu, so we don't error out if you have to save and reload
            //particularly useful in the spawnSettler use case where it's pretty much required
            //if (!everActivated) return;
        if (SceneManager.GetActiveScene().name.ToString() != "GameScene")
        {
            //Log("resetting ConsoleCommands values!");
            everActivated = false;
            //set all to null
            canvas = null;
            chat = null;
            cheatBox = null;
            player = null;
            //delete command_text gameobject
            if (command_text != null)
                Destroy(command_text);
            
            toggled_once = false;
            return;

        }


        if (!toggled_once)
            return;
        input = cheatBox.text; 
        
        
        
        //return is where we process everything, so we can bypass that
        //damned green send button I can't hijack
        if (Input.GetKey(KeyCode.Return) && ticker == 0)
        {
            ticker = 10;
            Cheats cheats = Cheats.instance;

            if (input == "giveAll")
            {
                for (int i = 0; i < Inventory.instance.itemDatas.Length; i++)
                {
                    ItemType itemType = (ItemType)i;
                    if (itemType != ItemType.People && itemType != ItemType.Happiness && itemType != ItemType.Golden_Retriever && itemType != ItemType.Health && itemType != ItemType.Hunger && itemType != ItemType.Star && itemType != ItemType.Power)
                    {
                        Inventory.instance.AddItem((ItemType)i, 999, true);

                    }
                }
                Log("Given every item!");

            }                   //example: giveAll

            else if (input == "help" || input == "help 1")
            {
                help = true;
                //set command_text alignment to center
                command_text.alignment = TextAlignmentOptions.Center;
                command_text.fontSize = 30;
                command_text.margin = new Vector4(0, 0, 0, 0);
                command_text.rectTransform.anchoredPosition = new Vector2(0,0);
                //set the text area to the size of the screen

                command_text.text = "Commands: \n" +
                    "<b><color=green>giveAll</color> - gives you every item in the game \n" +
                    "<color=green>give</color> <color=yellow>[item] [amount]</color> - gives you the item you specify, Capitalize first letter! \n" +
                    "<color=green>difficulty</color> <color=yellow>[number]</color> - sets the difficulty of the game \n" +
                    "<color=green>godmode</color> <color=yellow>[true or false]</color> - makes you invincible \n" +
                    "<color=green>help</color> <color=yellow>[number 1 to 2]</color> - shows this message \n" +
                    "<color=green>noFire</color> - extinguishes all fires \n" +
                    "<color=green>yesFire</color> - lights all fires \n" +
                    "<color=green>buildBoatyard</color> - builds a boatyard \n" +
                    "<color=green>restoreAll</color> - restores all settlers \n" +
                    "<color=green>spawnChickenBoat</color> - spawns a chicken boat \n" +
                    "<color=green>spawnBot</color> - spawns a bot \n" +
                    "<color=green>spawnMouse</color> - spawns a mouse \n" +
                    "<color=green>spawnSailorBoat</color> - spawns a sailor boat \n" +
                    "<color=green>spawnSettler</color> - spawns a settler, SAVE AND RELOAD AFTER \n" +
                    "<color=red>SPAWNSETTLER WILL RUIN THE PLACEMENT OF ALL NPCS, YOU HAVE BEEN WARNED.</color>\n" +
                    "<color=green>scale</color> <color=yellow>[number 0.1 to ?]</color> - scales the game \n" +
                    "<color=green>spawnWitch</color> - spawns the Witch trader \n" +
                    "<color=green>spawnSteelTrader</color> - spawns the Steel trader \n" +
                    "<color=green>spawnTrader</color> - spawns the Trader (may not work)\n" +
                    "<color=green>spawnPenguin</color> - spawns a penguin at your position \n" +
                    "<color=green>respawn</color> <color=yellow>[number 0 to 6]</color> - respawns the character at different spots\n" +
                    "<color=green>easyGather</color> <color=yellow>[true or false]</color> - starts gathering all trash \n" +
                    "<color=red>PRESS / AGAIN TO REMOVE THIS POPUP</color></b>";

            }

            else if (input.StartsWith("give"))
            {

                //slice input into 0 1 and 2
                string[] inputs = input.Split();
                Inventory inventory = FindObjectOfType<Inventory>();
                //convert string to int

                int amount = int.Parse(inputs[2]);

                Logger.LogInfo($"{inputs[0].ToString()}, {inputs[1].ToString()}, {inputs[2].ToString()}");

                for (int i = 0; i < Inventory.instance.itemDatas.Length; i++)
                {
                    ItemType itemType = (ItemType)i;

                    //if the name of the item matches inputs[1].toString()
                    if (itemType.ToString() == inputs[1].ToString() && amount > 0)
                    {


                        //add item to inventory
                        Inventory.instance.AddItem((ItemType)i, amount);
                        //log the item
                        Log($"Added {amount} {inputs[1]} to inventory!");


                    }

                }


            }        //example: give Cheese 100

            else if (input == "clear")
            {
                Inventory inventory = FindObjectOfType<Inventory>();
                Log($"{Inventory.instance.items.Count} is inventory");
                if (Inventory.instance != null)
                {
                    for (int i = Inventory.instance.items.Count - 1; i >= 0; i--)
                    {
                        Inventory.instance.items.RemoveAt(i);
                    }
                    Inventory.instance.onInventoryChanged();
                }
                else
                {
                    Log("cannot find inventory, this is a bug haha");
                }
                
            }

            else if (input.StartsWith("difficulty"))
            {
                string[] inputs = input.Split();
                difficulty = int.Parse(inputs[1]);

                if (difficulty < 0)
                {

                    difficulty = 0;
                }
                difficult = true;
                Log("Difficulty set to " + difficulty + " || default is 4");

            }  //example: difficulty 10

            else if (input.StartsWith("godmode"))
            {
                bool isTrue = bool.Parse(input.Split()[1]);
                godmode = isTrue;
                Log("Godmode set to " + godmode);
            }     //example: godmode true

            else if (input == "noFire")
            {
                Log("Putting out all fires!");
                foreach (Deck deck in BuildingManager.instance.decks)
                {
                    deck.buildingScript.fireDuration = 0f;
                }
                foreach (BuildingScript buildingScript in BuildingManager.instance.buildings)
                {
                    buildingScript.fireDuration = 0f;
                }
            }               //example: noFire

            else if (input == "someFire")
            {
                Log("20% chance of fire");
                RiotManager riotManager = FindObjectOfType<RiotManager>();
                //choice can be 0 or 1
                

                foreach (Deck deck in BuildingManager.instance.decks)
                {
                    int choice = Random.Range(0, 10);
                    if (choice <= 8) return;
                    riotManager.SetOnFire(deck.buildingScript, FireCause.Lightning);
                }
                
            }

            else if (input == "yesFire")                 //example: yesFire
            {
                Log("Fire has been set!");
                RiotManager riotManager = FindObjectOfType<RiotManager>();

                foreach (Deck deck in BuildingManager.instance.decks)
                {
                    riotManager.SetOnFire(deck.buildingScript, FireCause.Lightning);
                }
                foreach (BuildingScript buildingScript in BuildingManager.instance.buildings)
                {
                    riotManager.SetOnFire(buildingScript, FireCause.Lightning);
                }
            }              //example: yesFire

            else if (input == "buildBoatyard")
            {
                cheats.BuildBoatyard();
            }        //example: buildBoatyard

            else if (input == "restoreAll")
            {
                
                
                foreach (Character character in Toolbox.instance.characterManager.characters)
                {
                    character.hunger = (character.thirst = (character.happiness = (character.health = (character.rest = 100f))));
                }
                
            }           //example: restoreAll

            else if (input == "spawnChickenBoat")
            {
                cheats.SpawnChickenBoat();
            }     //example: spawnChickenBoat

            else if (input == "spawnBot")
            {
                cheats.SpawnBot();
            }             //example: spawnBot

            else if (input == "spawnMouse")
            {
                cheats.SpawnMouse();
            }           //example: spawnMouse

            else if (input == "spawnSailorBoat")
            {
                cheats.SpawnSailorBoat();
            }      //example: spawnSailorBoat

            else if (input == "spawnSettler")
            {
                MessageScript.instance.displayMessageDirectly("Spawning Settler, YOU MUST SAVE AND RELOAD OR LOG FILLS WITH ERRORS");
                Toolbox.instance.characterManager.editorSpawnCharacter();
                //set all characters transform.position to 0,0,0
                foreach (Character character in Toolbox.instance.characterManager.characters)
                    character.transform.position = Vector3.zero;
            }         //example: spawnSettler

            else if (input.StartsWith("easyGather"))
            {
                
                gathering = bool.Parse(input.Split()[1].ToString());
                Log("easyGathering set to " + gathering);
            }

            /*else if (input == "spawnZooey")
            {
                //no
            }  */         //zooey causes hella problems so I took the code to spawn her out and am disabling

            else if (input.StartsWith("scale"))
            {
                string[] inputs = input.Split();
                Logger.LogInfo(inputs[1] + " is the given scale");
                float scale = float.Parse(inputs[1]);
                Logger.LogInfo(scale + " is the given scalefdsafdsafdsafdsafdsafsdafasfdsafdsafdsafdafsafsdafasfdsafsda");
                if (scale < 0.1)
                    scale = 0.1f;

                foreach (Character character in Toolbox.instance.characterManager.characters)
                {
                    character.SetScale(scale);
                    

                }

                foreach (var character in FindObjectsOfType<Character>())
                {
                    character.SetScale(scale);
                }

                Log($"Set character scale to {scale}!");



            }       //example: scale 10
            
            else if (input == "spawnWitch")
            {
                Toolbox.instance.boatManager.SpawnWitchTrader();
            }           //example: spawnWitch

            else if (input == "spawnSteelTrader")
            {
                Toolbox.instance.boatManager.SpawnFirstSteelTrader(true);
            }     //example: spawnSteelTrader

            else if (input == "spawnTrader")
            {
                Toolbox.instance.boatManager.SpawnNewTraderBoat(1,0,true);
            }          //example: spawnTrader

            else if (input == "pollution")
            {
                isPollutionEnabled = !isPollutionEnabled;
            }

            else if (input == "pickupRange")
            {
                PlayerManager.LocalPlayerInstance.GetComponent<PlayerScript>().baseInteractionDistance = 999f;
            }

            else if (input == "infinitePower")
            {
                infPower = true;
            }

            else if (input == "spawnPenguin")
            {
                
                CharacterManager characterManager = FindObjectOfType<CharacterManager>();
                
                characterManager.SpawnNewPenguin(player.transform.position);


            }         //example: spawnPenguin

            else if (input.StartsWith("respawn"))
            {
                PlayerScript playerScript = FindObjectOfType<PlayerScript>();
                playerScript.Respawn(int.Parse(input.Split()[1]));
                Log("spawning player at postion " + input.Split()[1]);
            }     //this spawns you on the dif islands
            
            else if (input.StartsWith("fireDamage"))
            {
                bool choice = bool.Parse(input.Split()[1]);
                doesFireDmg = choice;
                Log($"<color>I <i>REALLY</i> do not recommend using this</color>\nWhen an object is destroyed the game <i>FILLS</i> with errors and you will\nshortly crash the game.\nFire Damage set to {doesFireDmg}");
            }  //again, REALLY don't recommend using this

            else if (input.StartsWith("netMax"))
            {
                int choice = int.Parse(input.Split()[1]);
                Net[] nets = FindObjectsOfType<Net>();
                if (choice < 1)
                {
                    choice = 1;
                }
                for (int i = 0; i < nets.Length; i++)
                {
                    
                    nets[i].maxTrash = choice;
                }
                Log("Max trash of nets set to " + choice);
            }      //example: netMax 10
                //number is how many ya want it to hold
            else if (input.StartsWith("trashSpawnInterval")) //example: trashSpawnInterval 0.1
                //the smaller the number, the faster it spawns
            {
                
                float choice = float.Parse(input.Split()[1]);
                if (choice <0)
                {
                    choice = 0;
                }

                trashSpawning = true;
                trashSpawnRate = choice;
                
                Log($"Trash spawn interval set to {choice}");
            }
            
            else if (input == "unlockAllResearch")
            {
                for (int i = 0; i < Game.current.researchLevels.Length; i++)
                {
                    Game.current.researchLevels[i] = 1;
                }
                
                ResearchManager researchManager = Toolbox.instance.researchManager;
                foreach (ResearchData researchData in researchManager.datas)
                {
                    Game.current.researchLevels[(int)researchData.researchType] = researchData.maxLevel - 1;
                    researchManager.CompleteResearch(researchData.researchType);
                }
                
            }
            
            else if (input.StartsWith("researchSpeed"))
            {
                int choice = int.Parse((input.Split()[1]));
                if (choice < 1)
                    choice = 1;
                Toolbox.instance.researchManager.researchSpeed = choice;
                Log($"Set research speed to {choice}");
            }

            else if (input.StartsWith("enableContent"))
            {
                GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                for (int i = 0; i < allObjects.Length; i++)
                {
                    if (allObjects[i].name == "Island3_AstroIsland")
                    {
                        allObjects[i].SetActive(true);
                        Log("Enabled science lab");
                    }
                    if (allObjects[i].name == "Nest_07")
                    {
                        allObjects[i].SetActive(true);
                        Log("Enabled bird nest");
                    }
                    if (allObjects[i].name == "Dolphin")
                    {
                        allObjects[i].SetActive(true);
                        Log("Enabled dolphin");
                    }
                    if (allObjects[i].name == "SM_Env_Flat_Sand_02")
                    {
                        allObjects[i].SetActive(true);
                        Log("Enabled sand bank/all other objects");
                    }

                }
                Log("Done loading objects!");
                
            }
            
            else if (input.StartsWith("flight"))
            {
                bool choice = bool.Parse((input.Split()[1]));

                if (choice == true || choice == false)
                {
                    if (choice)
                    {
                        
                        //set player's rigidbody to kinematic
                        Rigidbody rb = player.GetComponent<Rigidbody>();
                        rb.isKinematic = true;
                        Log("This will fill log with errors, don't abuse it!");
                        flight = true;
                    }
                    if (!choice)
                    {
                        
                        //set player's rigidbody to kinematic
                        Rigidbody rb = player.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        flight = false;
                    }
                    
                    
                }
                else
                {
                    Log("command error> true or false only");
                }
                

                
            }
            
            else if (input == "spawnTurret")
            {
                BuildingManager buildingManager = FindObjectOfType<BuildingManager>();

                BuildingData bd;
                foreach (BuildingData buildingData in BuildingManager.instance.buildingDatas)
                {
                    if (GetName(buildingData) == "Turret")
                    {
                        bd = buildingData;
                        buildingManager.buildingToBuild = bd;
                        //buildingManager.Build(bd);
                        Log("try building it now");
                    }


                }


                    


                     //buildingManager.buildingToBuild = "Air_Turret";*/

            }

            else if (input == "help 2")
            {
                //set command_text alignment to center
                command_text.alignment = TextAlignmentOptions.Center;
                command_text.fontSize = 30;
                command_text.margin = new Vector4(0, 0, 0, 0);
                command_text.text = "Commands: \n" +
                    "<b><color=green>fireDamage</color> <color=yellow>[true or false]</color> - enables fire damage. <color=red><i>I do not recommend this</color></i> \n" +
                    "<color=green>netMax</color> <color=yellow>[number 0 to ?]</color>- sets max amount a net can gather \n" +
                    "<color=green>trashSpawnInterval</color> <color=yellow>[number 0 to ?]</color> - sets how often trash/fish spawn \n" +
                    "<color=green>unlockAllResearch</color> - unlocks all research without setting cheating flag\n" +
                    "<color=green>researchSpeed</color> <color=yellow>[num 1 to ?]</color> - sets research speed\n" +
                    "<color=green>enableContent</color> - enables disabled content\n" +
                    "<color=green>flight</color> <color=yellow>[true or false]</color> - enables flight/ fills log w/ errors\n" +
                    "<color=green>clear</color> - clears inventory\n" +
                    "<color=green>pollution</color> - toggles pollution\n" +
                    "<color=green>infinitePower</color> - toggles infinitePower\n" +
                    "<color=green>pickupRange</color> - sets pickupRange to 999\n" +
                    "<color=red>PRESS / AGAIN TO REMOVE THIS POPUP</color></b>";
                help = true;
            }
            
            cheatBox.gameObject.SetActive(!cheatBox.gameObject.activeSelf);
            cheatBox.text = "";
            input = "";
            if (cheatBox.gameObject.activeSelf)
            {
                cheatBox.Select();
                cheatBox.ActivateInputField();

            }
            if (!cheatBox.gameObject.activeSelf && !help)
            {
                image.color = transparent;
                command_text.text = "";
                if (myPanel != null)
                    Destroy(myPanel);   
                if (suggestions != null)
                    Destroy(suggestions);
                everActivated = false;
            }

        }

        //if presses numpad5
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Log("building spawn is not ready yet");
            //working on spawningn buildings, not quite there yet
            //gotta figure out how we're generating buildingData
            
            
            
            /*BuildingData buildingData = buildingData[1];
            base.enabled = true;

            BuildingManager.instance.buildingToBuild = BuildingData;
            BuildingManager.instance.enterBuildingMode();
            foreach (object obj in base.transform.parent)
            {
                Transform transform = (Transform)obj;
                BuildingButton component = transform.GetComponent<BuildingButton>();
                component.glow.SetActive(transform.gameObject == base.gameObject);
                component.enabled = transform.gameObject == base.gameObject;
            }*/
        }
        

        //suggestions and autocomplete
        if (cheatBox.gameObject.activeSelf)
        {
            //Command text is the autocomplete's textmeshprougui object, to display the choices while typing
            //the suggestions, for lack of a better way to put it
            command_text.text = "";
            if (input.Split().Length > 1)
            {
                //Log(input.Split().Length.ToString());
                textToDisplay = input.Split();
                for (int i = 0; i < items.Length; i++)
                {
                    //Log(textToDisplay[0].ToString());
                    if (textToDisplay[1] != "" && textToDisplay[1] != " " && items[i].StartsWith(textToDisplay[1].ToString()))
                    {
                        command_text.text = command_text.text + " " + items[i];
                        //Log(command_text.text);
                    }
                }

                //this is for autocomplete for the second word
                if (Input.GetKeyDown(KeyCode.Tab)) 
                {
                    string[] strings = command_text.text.Split();

                    string first_word = input.Split()[0].ToString();
                    
                    string needed_word = strings[1].ToString();
                    
                    

                    input = first_word + " " + needed_word;
                    
                    cheatBox.text = input;
                    cheatBox.caretPosition = input.Length + 1;

                    

                }
            }
            else
            {
                //suggestion for first word
                if (input == "") return;
                for (int i = 0; i < first_commands.Length; i++)
                {
                    //Log(textToDisplay[0].ToString());
                    if (first_commands[i].StartsWith(input.ToString()))
                    {
                        command_text.text = command_text.text + " " + first_commands[i];
                        //Log(command_text.text);
                    }
                }

                //autocomplete for first word
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    string[] strings = command_text.text.Split();
                    
                    string needed_word = strings[1].ToString();
                    input = needed_word;
                    cheatBox.text = input;
                    cheatBox.caretPosition = input.Length+1;
                    
                }
            }
        }



        //harmonyx patch for public bool CanBuildSpecifiedBuildingTypeHere(BuildingData buildingToBuild)
        


    }


 

    
    

}

//harmonyx patch for public bool CanBuildSpecifiedBuildingTypeHere(BuildingData buildingToBuild)