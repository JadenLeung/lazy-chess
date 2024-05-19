using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{

    public GameObject chesspiece;

    public GameObject movePlate;

    //Positions and team for each chesspiece
    private GameObject[,] positions = new GameObject[8, 8];
    public bool[,] pieceplatepositions = new bool[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    public GameObject restartbutton;
    public GameObject undobutton;
    public GameObject redobutton;
    public GameObject homebutton;

    public AudioSource source;
    public AudioClip moveclip;
    public AudioClip captureclip;
    public AudioClip movecaptureclip;
    public AudioClip capturecaptureclip;
    public AudioClip promoteclip;
    public AudioClip promotecaptureclip;
    public AudioClip castleclip;
    public AudioClip castlecaptureclip;
    public AudioClip castlepromoteclip;
    public AudioClip winclip;

    public AudioClip movepromoteclip;
    public AudioClip capturepromoteclip;
    public AudioClip promotepromoteclip;

    public string recentmove = "move";
    public GameObject readybutton;
    public GameObject menubutton;
    public GameObject continuebutton;
    public GameObject previousbutton;
    public GameObject [] promotes = new GameObject[6];
    public string currentPlayer = "white";
    public int [,] platepositions = new int [8, 8];

    public bool gameOver = false;
    public int enpassant = -1;
    public int actualpassantx = -1;
    public int actualpassanty = -1;
    public int preyx = -1;
    public int preyy = -1;
    public int predatorx = -1;
    public int predatory = -1;
    public string curposition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    public string metacurposition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    public int savepositionw = 16;
    public int savepositionb = 16;
    public int curmovegoal1 = 5;
    public int curmovegoal2 = 7;
    public int moves = 0;
    public string goal = "moves";
    public string title = "Level Select";
    public List<string> undo = new List<string>();
    public List<string> redo = new List<string>();
    public List<string> allmoves = new List<string>();
    public List<string> redomoves = new List<string>();

    public List<int> undospent = new List<int>();
    public List<int> redospent = new List<int>();
    public int whitekings = 1;
    public bool pawnpromote = false;
    public bool setpiece = false;
    public int setpiecex = -1;
    public int setpiecey = -1;
    public int budget = 16;
    public int spent = 0;
    public bool actuallypromote = false;
    public string piecepromote = "Wqueen";
    public int levelnum = 0;
    public bool deleteLines = false;

    public string mode = "ingame"; 
    public bool pieceremoved = false;
    public string savepieces = "";
    public int savespent = 0;
    public GameObject input;
    public int tutorialnum = 0;

    public string[,] levels  = {{"`", "Kyyyyyyy/1yyyyyyy/1yyyyyyy/1yyyyyyy/1yyyyyyy/1yyyyyyy/1yyyyyyy/8", "-1", "-1", "levels", "Level Select", "moves",""}, // w - - 0 1
    {"f", "8/8/8/8/8/8/8/8", "10", "12", "ingame", "Tutorial", "moves",""},
    {"f", "8/8/5k2/8/3N4/8/8/8", "4", "6", "ingame", "Horsin' Around", "moves",""},
    {"f", "6rk/6rr/8/8/8/8/RR6/R7", "3", "4", "ingame", "Prying Away", "moves",""},
    {"f", "8/1q4q1/1k1q1q2/8/8/8/3q2q1/2q4Q", "5", "7", "ingame", "Safe Zone", "moves",""},
    {"f", "4rr1r/4rr1k/8/4rr1r/4rr1r/4rr1r/R3rr1r/R5K1", "4", "5", "ingame", "Hungry King", "moves",""}, 
    {"f", "rnbq1rk1/ppppppbp/5np1/8/8/8/PPPPPPPP/RNBQKBNR", "6", "8", "ingame", "The Fianchetto", "moves",""},
    {"f", "6r1/4k1n1/3n3n/4n1n1/5r2/3p1p2/4P3/8", "7", "8", "ingame", "Pawn's Journey", "moves",""},
    {"f", "k7/8/8/pppppppp/8/P1K5/P7/8", "10", "11", "ingame", "The Great Wall", "moves",""},
    {"f", "8/8/8/8/8/8/8/8", "10", "12", "ingame", "Piece Priority Tutorial", "moves",""},
    {"f", "Q3qqkq/4qqqq/8/8/8/8/8/Q7", "2", "3", "ingame", "Eat The Front", "moves",""},
    {"f", "ppppkppp/pppppppp/pppppppp/8/8/8/8/RNBQKBNR", "5", "6", "ingame", "Pawn Wasteland", "moves",""},
    {"f", "5ppk/n4p1p/6n1/2r5/8/8/2P5/1B1B4", "10", "12", "ingame", "In My Way", "moves",""},
    {"f", "kkkkkkkk/8/8/1BBBBBK1/8/8/8/8", "14", "15", "ingame", "Polymonarchy", "moves",""},
    {"f", "4ppkq/4pp1q/4pp1P/3Ppp1P/4pp1P/4pp1P/4ppPP/6PK", "15", "18", "ingame", "Zwischenzug", "moves",""},
    {"f", "q1k5/1q6/2q5/3q4/4N3/5N2/6N1/5K1N", "7", "8", "ingame", "God Save The King", "moves",""},
    {"f", "rnbqkbnr/pppppppp/8/PPPPPPPP/8/8/8/8/", "11", "12", "ingame", "The One To Get Through", "moves",""},
    {"f", "KKKK1KKK/PPPPPPPP/PPPPPPPP/PPPPPPPP/PPPPPPPP/8/pppppppp/rnbqkbnr", "13", "20", "ingame", "Backwards Pawn", "moves",""},
    {"f", "4rkrr/5rrr/4rrrr/4rnrr/NNN5/8/8/1B6", "7", "10", "ingame", "Lazer Beams", "moves",""},
    {"f", "k7/bp6/pbpbpbpb/pppppppp/pppppppp/8/1B3N1P/2B5", "22", "24", "ingame", "The Immortal Knight And The Distractor Bishops", "moves",""},
    {"f", "qqqqkqqq/qqqqqqqq/qqqqqqqq/8/8/8/PPPPPPPP/RNBQKBNR", "5", "7", "ingame", "Nelson", "moves",""},
    {"f", "NBpbbbkb/1pprbbbb/2rbpbpb/B1p1p1pp/1p6/3pP3/2pP1P2/1NP3B1", "8", "10", "ingame", "Luring The Attacker", "moves",""},
    {"f", "8/5P1k/3KKK1k/8/8/8/8/8", "15", "18", "ingame", "TEST_LEVEL", "moves",""},
    {"f", "6rk/7r/7r/8/P1P1P3/P1P1P3/P1P1P3/P1P1P3", "18", "22", "ingame", "Pushing P", "moves",""},
    {"f", "rnbqkbnr/pppppppp/8/8/8/8/xxxxxxxx/xxxxxxxx", "4", "5", "setpiece", "Under Budget", "points", "6"},
    {"f", "k4r2/bbbp4/bbbp3p/brrp3p/4pr1p/5x1x/x2x4/x3b3", "7", "8", "setpiece", "Sac Attack", "points", "10"},
    {"f", "kkkkkkkk/8/xxxxxxxx/8/8/8/8/8/", "8", "10", "setpiece", "Royal Flush", "points", "12"},
    {"f", "bbkbbbbq/1q1qpp2/2nqq12/8/8/1x5x/4x2x/4K3", "8", "11", "setpiece", "Secret Escort", "points", "28"},
    {"f", "qqqqkqqq/rrrrrrrr/rrrrrrrr/rrrrrrrr/8/xxxxx3/xxxxx3/xxxxx2K", "14", "16", "setpiece", "Safety First", "points", "40"},
    {"f", "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", "4", "6", "sandbox", "Sandbox Mode", "none",""}}; 

    public string charlist = "`1234567890-=qwetyuiop[]asdfghjkl;";

    public static int cool = 5;
    public static string [] medals;
    // Start is called before the first frame update
    void Start()
    {
        if(Menu.Mmedals == null){
            medals =  new string[levels.Length]; 
            for(int i = 0; i < medals.Length; i++){
                medals[i] = "gray";
            }
            Menu.setMedals();
        }
        else{
            medals = Menu.Mmedals;
        }
        mode = "levels";
        input = GameObject.FindGameObjectWithTag("Input");  
        //restartbutton = GameObject.FindGameObjectWithTag("RestartButton");
        readybutton = GameObject.FindGameObjectWithTag("ReadyButton");
        undobutton = GameObject.FindGameObjectWithTag("UndoButton");
        redobutton = GameObject.FindGameObjectWithTag("RedoButton");
        promotes[0] = GameObject.FindGameObjectWithTag("PromoteK");
        promotes[1] = GameObject.FindGameObjectWithTag("PromoteQ"); 
        promotes[2] = GameObject.FindGameObjectWithTag("PromoteR");
        promotes[3] = GameObject.FindGameObjectWithTag("PromoteB");
        promotes[4] = GameObject.FindGameObjectWithTag("PromoteN");
        promotes[5] = GameObject.FindGameObjectWithTag("PromoteP");
        restartbutton.GetComponent<Button>().onClick.AddListener(() => restart());
        undobutton.GetComponent<Button>().onClick.AddListener(Undo);
        redobutton.GetComponent<Button>().onClick.AddListener(Redo);
        readybutton.GetComponent<Button>().onClick.AddListener(ready);
        homebutton.GetComponent<Button>().onClick.AddListener(home);
        promotes[0].GetComponent<Button>().onClick.AddListener(() => setPromoteButton("Wking"));
        promotes[1].GetComponent<Button>().onClick.AddListener(() => setPromoteButton("Wqueen"));
        promotes[2].GetComponent<Button>().onClick.AddListener(() => setPromoteButton("Wrook"));
        promotes[3].GetComponent<Button>().onClick.AddListener(() => setPromoteButton("Wbishop"));
        promotes[4].GetComponent<Button>().onClick.AddListener(() => setPromoteButton("Wknight"));
        promotes[5].GetComponent<Button>().onClick.AddListener(() => setPromoteButton("Wpawn"));
        
        title = "Level Select";
        promoteVisibility(false);
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "No level selected";
        
        input.SetActive(false);
        metacurposition = levels[0,1];
        setStringPosition(metacurposition);
        curposition = metacurposition;
        undo.Add(getStringPosition());
        hideMainButtons();

        for(int i = 0; i < levels.GetLength(0); i++){
            levels[i,0] = charlist[i] + "";
        }
    }

    public GameObject Create(string name, int x, int y){
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }
    public void setPromoteButton(string name){
        if(setpiece){
            piecepromote = name;
            setpiece = false;
            GameObject[] playerWhite2 = new GameObject[playerWhite.Length + 1];
            if(name == "Wpawn") spent += 1;
            if(name == "Wknight") spent += 3;
            if(name == "Wbishop") spent += 3;
            if(name == "Wrook") spent += 5;
            if(name == "Wqueen") spent += 9;
            undospent.Add(spent);
            redospent.Clear();
            playerWhite2[playerWhite.Length] = Create(name, setpiecex, setpiecey);
            for(int i = 0; i < playerWhite.Length; i++){
                playerWhite2[i] = playerWhite[i];
            }
            playerWhite = playerWhite2;
            SetPosition(playerWhite[playerWhite.Length - 1]);
            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate"); 
            for(int i = 0; i < movePlates.Length; i++){
                if(movePlates[i].GetComponent<MovePlate>().ispressed == true)
                    movePlates[i].GetComponent<MovePlate>().ispressed = false;
            }
            pieceremoved = false;
            restartbutton.SetActive(true);
            readybutton.SetActive(true);
            //undobutton.SetActive(true);
            //redobutton.SetActive(true);
            playSound("promote");

            //pieceplatepositions[setpiecex, setpiecey] = false;
            undo.Add(getStringPosition());
            redo.Clear();
        }
        else{
            if(currentPlayer == "white")
                piecepromote =  "W" + name.Substring(1);
            else
                piecepromote =  "B" + name.Substring(1);
            pawnpromote = false;
            actuallypromote = true;
        }
    }
    public void SetPosition(GameObject obj){
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }
    public void SetPositionEmpty(int x, int y){
        positions[x,y] = null;
    }
    public GameObject GetPosition(int x, int y){
        return positions[x,y];
    }
    public bool PositionOnBoard(int x, int y){
        if(x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true; 
    }

    public string GetCurrentPlayer(){
        return currentPlayer;
    }

    public bool IsGameOver(){
        return gameOver;
    }

    public void NextTurn(){
        if(mode != "levels"){
            restartbutton.SetActive(true);
            undobutton.SetActive(true);
            redobutton.SetActive(true);
        }

        if(mode == "sandbox"){
            if(numPiece("Wking") == 0)
                Winner("black");
                
            playSound(recentmove);
            undo.Add(getStringPosition());
            redo.Clear();
            //actualpassantx = -1;
            //actualpassanty = -1;
            //enpassant = -1;
        }
        if(currentPlayer == "white"){
            moves++;  
            currentPlayer = "black";
        } else{
            currentPlayer = "white";
            undo.Add(getStringPosition());
            redo.Clear();
        }

    }
    public string removeX(string s){
        string s2 = "";
        for(int i = 0; i < s.Length; i++){
            if(s[i] == 'x')
                s2 += "1";
            else
                s2 += s[i];
        }
        return renderStringPosition(s2);
    }
    public int pawns(string name){
        if(name[1] == 'p') return 1;
        if(name[2] == 'n') return 3;
        if(name[1] == 'b') return 3;
        if(name[1] == 'r') return 5;
        if(name[1] == 'q') return 9;
        return 0;
    }
    public void removePiece(int x, int y){
        if(positions[x,y] != null){
            if(mode == "setpiece"){
                spent -= pawns(positions[x,y].GetComponent<Chessman>().name);
            }
            Destroy(positions[x,y]);
            pieceremoved = true;
        }
    }
    public void ready(){
        if(mode == "setpiece"){
            savepieces = getStringPosition();
            pawnpromote = false;
            gameOver = false;
            deleteLines = true;
            mode = "ingame";
            undobutton.SetActive(true);
            redobutton.SetActive(true);
            Debug.Log(undo[undo.Count-1]);
            Debug.Log(removeX(undo[undo.Count-1]));
            curposition = removeX(undo[undo.Count-1]);
            moves = 0;
            undo.Clear();
            redo.Clear();
            currentPlayer = "white";   
            undo.Clear();
            undo.Add(curposition);
            GameObject.Find("ReadyButton").GetComponentInChildren<Text>().text = "Resetup";
            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate"); 
            for(int i = 0; i < movePlates.Length; i++){
                    Destroy(movePlates[i]);
            }
            pieceplatepositions = new bool[8, 8];
            savespent = spent;
        }
        else{ //reset
            curposition = savepieces;
            spent = savespent;
            GameObject.Find("ReadyButton").GetComponentInChildren<Text>().text = "Ready";
            mode = "setpiece";
            restart(true);
        }
    }
    public void restart(bool saveit = false){
        DestroyAllPlates(); 
        undospent.Clear();
        redospent.Clear();
        allmoves.Clear();
        redomoves.Clear();
        pawnpromote = false;
        gameOver = false;
        deleteLines = true;
        setpiece = false;
        promoteVisibility(false);
        pawnpromote = false;
        restartbutton.SetActive(true);
        undo.Clear();
        redo.Clear();
        currentPlayer = "white";   
        if(mode == "levels"){
            hideMainButtons();
        }else{
            showMainButtons();
        }
        
        if(mode == "ingame"){
            undobutton.SetActive(true);
            redobutton.SetActive(true);
        }
        else{
            if(saveit){
                spent = savespent;
                undospent.Add(spent);
            }
            else
                spent = 0;
        }
        moves = 0;
        if(mode != "levels")
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = false;
        else{
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "No level selected";
        }
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = false;
        if(mode == "ingame" || saveit)
            setStringPosition(curposition);
        else{
            curposition = metacurposition;
            setStringPosition(metacurposition);
        }
        undo.Add(curposition);
        if(title == "Horsin' Around")
            input.SetActive(true);
        else
            input.SetActive(false);
    }


    public void DestroyAllPlates(){
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate"); 
        for(int i = 0; i < movePlates.Length; i++){
                Destroy(movePlates[i]);
        }
    }
    public char pieceFromString(string pos, int px, int py){
        int x = 0;
        int y = 7;
        for(int i = 0; i < pos.Length; i++){
            Debug.Log(x + " " + y);
            if(x == px && y == py){
                return pos[i];
            }
            if(pos[i] - '0' >= 0 && pos[i] - '0' <= 7){
                int num = pos[i] - '0';
                x+=num;
                continue;
            }
            if(pos[i] == '/'){
                x = 0;
                y--;
                continue;
            }
            x++;
        }
        return 'f';
    }
    public bool Nevermoved(char piece, int px, int py){
        bool nevermoved = true;
        for(int i = 0; i < undo.Count; i++){
            nevermoved = nevermoved && pieceFromString(undo[i], px, py) == piece;
        }
        return nevermoved;
    }
    public void setStringPosition(string position){
        int lenw = 0;
        int lenb = 0;  
        for(int i = 0; i < position.Length; i++){
            if(position[i] <= 'Z' && position[i] >= 'A')
                lenw++;
            if(position[i] <= 'z' && position[i] >= 'a' && position[i] != 'x' && position[i] != 'y') //x means pieceselect, y means level
                lenb++;
        }

        enpassant = -1;
        actualpassantx = -1;
        actualpassanty = -1;

        pieceplatepositions = new bool[8, 8];
        DestroyAllPlates();
        

        for(int i = 0; i < playerWhite.Length; i++){
            Destroy(playerWhite[i]);
        }
        for(int i = 0; i < playerBlack.Length; i++){
            Destroy(playerBlack[i]);
        }
        playerWhite = new GameObject[lenw];
        playerBlack = new GameObject[lenb];

        int x = 0;
        int y = 7;
        int whiteindex = 0;
        int blackindex = 0;

        for(int i = 0; i < position.Length; i++){
            if(position[i] - '0' >= 0 && position[i] - '0' <= 7){
                int num = position[i] - '0';
                x+=num;
                continue;
            }
            if(position[i] == '/'){
                x = 0;
                y--;
                continue;
            }
            if(position[i] == 'k')
                playerBlack[blackindex++] = Create("Bking", x, y);
            else if(position[i] == 'q')
                playerBlack[blackindex++] = Create("Bqueen", x, y);
            else if(position[i] == 'r')
                playerBlack[blackindex++] = Create("Brook", x, y);
            else if(position[i] == 'b')
                playerBlack[blackindex++] = Create("Bbishop", x, y);
            else if(position[i] == 'n')
                playerBlack[blackindex++] = Create("Bknight", x, y);
            else if(position[i] == 'p')
                playerBlack[blackindex++] = Create("Bpawn", x, y);
            else if(position[i] == 'K')
                playerWhite[whiteindex++] = Create("Wking", x, y);
            else if(position[i] == 'Q')
                playerWhite[whiteindex++] = Create("Wqueen", x, y);
            else if(position[i] == 'R')
                playerWhite[whiteindex++] = Create("Wrook", x, y);
            else if(position[i] == 'B')
                playerWhite[whiteindex++] = Create("Wbishop", x, y);
            else if(position[i] == 'N')
                playerWhite[whiteindex++] = Create("Wknight", x, y);
            else if(position[i] == 'P')
                playerWhite[whiteindex++] = Create("Wpawn", x, y);
            x++;
        }
          x = 0;
         y = 7;
         for(int i = 0; i < metacurposition.Length && (mode == "setpiece" || mode == "levels"); i++){
            if(metacurposition[i] - '0' >= 0 && metacurposition[i] - '0' <= 7){
                int num = metacurposition[i] - '0';
                x+=num;
                continue;
            }
            else if(metacurposition[i] == '/'){
                x = 0;
                y--;
                continue;
            }else if((metacurposition[i] != 'x' && metacurposition[i] != 'y') || (metacurposition[i] == 'y' && levels.GetLength(0) <= (7-y)*7 + x)){
                x++;
                continue;
            }

            if((metacurposition[i] == 'x' || metacurposition[i] == 'y') && !pieceplatepositions[x,y]){
                float x2 = x;
                float y2 = y;
                //Adjust by variable offset
                x2 *= 0.7f;
                y2 *= 0.7f;

                //Add constants (pos 0,0)
                x2 += -2.45f;
                y2 += -2.45f;

                //Set actual unity values
                GameObject mp = Instantiate(movePlate, new Vector3(x2, y2, -3.0f), Quaternion.identity);
                mp.GetComponent<SpriteRenderer>().sortingOrder = 0;
                MovePlate mpScript = mp.GetComponent<MovePlate>();
                if(metacurposition[i] == 'x')
                    mpScript.pieceplate = true;
                else
                    mpScript.levelplate = true;
                mpScript.SetReference(chesspiece);
                mpScript.SetCoords(x, y);

                pieceplatepositions[x,y] = true;  
                x++;
                continue;
            }

            x++;
        }
        for(int i = 0; i < playerBlack.Length; i++){
            SetPosition(playerBlack[i]);
        } 
        for(int i = 0; i < playerWhite.Length; i++){
            SetPosition(playerWhite[i]);
        }
    }
    public void cancelPiecePlate(){
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate"); 
        for(int i = 0; i < movePlates.Length; i++){
            if(movePlates[i].GetComponent<MovePlate>().ispressed == true)
                movePlates[i].GetComponent<MovePlate>().ispressed = false;
        }
        if(pieceremoved){
            undo.Add(getStringPosition());
            redo.Clear();
            undospent.Add(spent);
            redospent.Clear();
        }
        setpiece = false;
        promoteVisibility(false);
        restartbutton.SetActive(true);
    }
    public void removeListDuplicates(){
        for(int i = 1; i < undo.Count; i++){
            if(undo[i] == undo[i-1]){
                undo.RemoveAt(i);
                i--;
            }
        }
        for(int i = 1; i < redo.Count; i++){
            if(redo[i] == redo[i-1]){
                redo.RemoveAt(i);
                i--;
            }
        }
        for(int i = 1; i < undospent.Count; i++){
            if(undospent[i] == undospent[i-1]){
                undospent.RemoveAt(i);
                i--;
            }
        }
        for(int i = 1; i < redospent.Count; i++){
            if(redospent[i] == redospent[i-1]){
                redospent.RemoveAt(i);
                i--;
            }
        }
    }
    public string getStringPosition(){
        string str = "";
        for(int y = 7; y >= 0; y--){
            for(int x = 0; x < 8; x++){
                if(pieceplatepositions[x,y] && positions[x,y] == null)
                    str += 'x';
                else if(positions[x,y] == null)
                    str += "1";
                else{
                    char piece = positions[x,y].GetComponent<Chessman>().name[0];
                    if(piece == 'W'){
                        if(positions[x,y].GetComponent<Chessman>().name[2] == 'n')
                            str += positions[x,y].GetComponent<Chessman>().name[2].ToString().ToUpper();
                        else
                            str += positions[x,y].GetComponent<Chessman>().name[1].ToString().ToUpper();
                    }
                    else {
                        if(positions[x,y].GetComponent<Chessman>().name[2] == 'n')
                            str += positions[x,y].GetComponent<Chessman>().name[2].ToString();
                        else
                            str += positions[x,y].GetComponent<Chessman>().name[1].ToString();
                    }
                }
            }
            str += "/";
        }
        return renderStringPosition(str);
    } 
    public string renderStringPosition(string s){
        string s2 = "";
        int adder = 0;
        for(int i = 0; i < s.Length; i++){
            if(i < s.Length - 1 && s[i] - '0' >= 0 && s[i] - '0' <= 7 && s[i+1] - '0' >= 0 && s[i+1] - '0' <= 7){
                adder += (s[i] - '0');
            }
            else if(s[i] - '0' >= 0 && s[i] - '0' <= 7){
                adder += (s[i] - '0');
                s2 += adder.ToString();
            }
            else if(i < s.Length - 1 && s[i] == '/' && s[i+1] == '/')
                s2 += "/8";
            else {
                s2 += s[i];
                adder =  0;
            }
        }
        return s2;
    }
    public void playSound(string sound){
        if(sound == "move")
            source.PlayOneShot(moveclip);
        else if(sound == "capture")
            source.PlayOneShot(captureclip);
        else if(sound == "movecapture")
            source.PlayOneShot(movecaptureclip);
        else if(sound == "capturecapture")
            source.PlayOneShot(capturecaptureclip);
        else if(sound == "promote")
            source.PlayOneShot(promoteclip);
        else if(sound == "promotecapture")
            source.PlayOneShot(promotecaptureclip); 
        else if(sound == "movepromote")
            source.PlayOneShot(movepromoteclip); 
        else if(sound == "capturepromote")
            source.PlayOneShot(capturepromoteclip); 
        else if(sound == "promotepromote")
            source.PlayOneShot(promotepromoteclip); 
        else if(sound == "castle")
            source.PlayOneShot(castleclip); 
        else if(sound == "castlecapture")
            source.PlayOneShot(castlecaptureclip); 
        else if(sound == "castlepromote")
            source.PlayOneShot(castlepromoteclip); 
    }
    public bool validMouseLocation(int x, int y){
        return x >= 0 && x <= 7 && y >= 0 && y <= 7;
    }
    public void Update(){
        if(goal == "moves")
            GameObject.FindGameObjectWithTag("MovesText").GetComponent<Text>().text = "Total moves: " + moves;
        else
            GameObject.FindGameObjectWithTag("MovesText").GetComponent<Text>().text = "Material Points Used: " + spent;

        if(mode == "levels" || title == "Sandbox Mode" || title == "Tutorial" || title == "Piece Priority Tutorial"){
            GameObject.FindGameObjectWithTag("MovesText").GetComponent<Text>().enabled = false;
            GameObject.FindGameObjectWithTag("MovegoalText").GetComponent<Text>().enabled = false;
            if(mode == "levels" && (!(validMouseLocation(MouseLocation()[0], MouseLocation()[1])) || !pieceplatepositions[MouseLocation()[0], MouseLocation()[1]]))
                GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "Select a Level";
        }
        else{
            GameObject.FindGameObjectWithTag("MovesText").GetComponent<Text>().enabled = true;

            GameObject.FindGameObjectWithTag("MovegoalText").GetComponent<Text>().enabled = true;
        }
        GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>().enabled = true;

        if(title == "Tutorial" && tutorialnum < 7) { //actual tutor
            Text t = GameObject.FindGameObjectWithTag("TutorialText").GetComponent<Text>();
            if(tutorialnum == 0) t.text = "In Lazy Chess, you are the white pieces and are trying to capture the opponent's king.";
            continuebutton.SetActive(tutorialnum < 6);
            previousbutton.SetActive(tutorialnum > 0);
            GameObject.FindGameObjectWithTag("TutorialText").GetComponent<Text>().enabled = true;
        }
        else if(title == "Piece Priority Tutorial" && tutorialnum < 21) { //actual tutor
            Text t = GameObject.FindGameObjectWithTag("TutorialText").GetComponent<Text>();
            if(tutorialnum == 0) t.text = "In Lazy Chess, you are the white pieces and are trying to capture the opponent's king.";
            continuebutton.SetActive(tutorialnum < 20);
            previousbutton.SetActive(tutorialnum > 7);
            GameObject.FindGameObjectWithTag("TutorialText").GetComponent<Text>().enabled = true;
        }
        else {
            continuebutton.SetActive(false);
            previousbutton.SetActive(false);
            GameObject.FindGameObjectWithTag("TutorialText").GetComponent<Text>().enabled = false;
        }
        if(title == "Piece Priority Tutorial" && tutorialnum == 0){
            tutorialnum = 7;
            tutorialChanged();
        } 
        if(mode != "levels"){
            GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>().text = "Level " + levelnum + ": " + title;
            menubutton.SetActive(false);
        }
        else{
            GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>().text = title;
            menubutton.SetActive(true);
        }
        homebutton.SetActive(mode != "levels");

        if(goal == "moves")
            GameObject.FindGameObjectWithTag("MovegoalText").GetComponent<Text>().text = "Gold: " + curmovegoal1 + " moves\nSilver: " + curmovegoal2 + " moves\nBronze: Completion";
        else
            GameObject.FindGameObjectWithTag("MovegoalText").GetComponent<Text>().text = "Gold: " + curmovegoal1 + " points\nSilver: " + curmovegoal2 + " points\nBronze: Completion";
        GameObject.FindGameObjectWithTag("SetupText").GetComponent<Text>().text = "Click on the highlighted squares to put down pieces. Press ready when you have finished.\nYou have used " + spent + "/" + budget + " total points of material allowed.";
        if(mode == "setpiece"){
            if(!setpiece) GameObject.FindGameObjectWithTag("SetupText").GetComponent<Text>().enabled = true;
            else GameObject.FindGameObjectWithTag("SetupText").GetComponent<Text>().enabled = false;
            undobutton.SetActive(false);
            redobutton.SetActive(false);   
            if(GameObject.Find("ReadyButton") != null)
                GameObject.Find("ReadyButton").GetComponentInChildren<Text>().text = "Ready";
            if(GameObject.Find("RestartButton") != null)
                GameObject.Find("RestartButton").GetComponentInChildren<Text>().text = "Reset";
        }
        else{
            GameObject.FindGameObjectWithTag("SetupText").GetComponent<Text>().enabled = false;
            if(GameObject.Find("ReadyButton") != null)
                GameObject.Find("ReadyButton").GetComponentInChildren<Text>().text = "Resetup";
            if(GameObject.Find("RestartButton") != null)
                GameObject.Find("RestartButton").GetComponentInChildren<Text>().text = "Restart";
        }

        if(metacurposition.Contains('x') && !setpiece && !pawnpromote) readybutton.SetActive(true);
        else readybutton.SetActive(false);

        if(mode == "ingame"){ 
            for(int i = 0; i < undo.Count; i++){
                if(undo[i].Contains('x')){
                    undo[i] = removeX(undo[i]);
                }
            }
            for(int i = 0; i < redo.Count; i++){
                if(redo[i].Contains('x')){
                    redo[i] = removeX(redo[i]);
                }
            }
        }
        
        removeListDuplicates();

        
        if(input.GetComponent<InputField>().isFocused) return;
        if(Input.GetKey("r")){
            restart();
        }
        if(Input.GetKey("z") && setpiece && (budget - spent >= 1)){
            setPromoteButton("Wpawn");
            return;
        }
        if(Input.GetKey("x") && setpiece && (budget - spent >= 3)){
            setPromoteButton("Wknight");
            return;
        }
        if(Input.GetKey("c") && setpiece && (budget - spent >= 3)){
            setPromoteButton("Wbishop");
            return;
        }
        if(Input.GetKey("v") && setpiece && (budget - spent >= 5)){
            setPromoteButton("Wrook");
            return;
        }
        if(Input.GetKey("b") && setpiece && (budget - spent >= 9)){
            setPromoteButton("Wqueen");
            return;
        }

        for(int i = 0; i < levels.GetLength(0); i++){
            if(setpiece) break;
            if(Input.GetKey(levels[i,0])){
                makeLevel(i);
            }
        }
        if(Input.GetKeyUp(KeyCode.DownArrow)){
            Undo();
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow)){
            Redo();
        }
        if(currentPlayer == "black" && mode != "sandbox"){ 
            preyx = -1;
            preyy = -1;
            predatorx = -1;
            predatory = -1;
            bool anyattack = false;
            for(int i = 0; i < playerBlack.Length; i++){
                if(playerBlack[i] == null) continue;
                anyattack = anyattack | (pieceAttack(playerBlack[i]));
            }
            Debug.Log(getPiece(predatorx, predatory) + " on (" + predatorx + "," + predatory + ") attacking " + 
            getPiece(preyx, preyy) + " on (" + preyx + "," + preyy + ")");

            if(anyattack && gameOver == false && getPiece(predatorx, predatory)[0] == 'B'){
                GameObject controller = GameObject.FindGameObjectWithTag("GameController");

                GameObject cp = controller.GetComponent<Game>().GetPosition(preyx, preyy);

                if(cp.name == "Wking"){
                    if(numPiece("Wking") == 1)
                        controller.GetComponent<Game>().Winner("black");
                } 
                if(cp.name == "Bking"){
                    if(numPiece("Bking") == 1)
                        controller.GetComponent<Game>().Winner("white");
                }

                if(cp != null)
                    Destroy(cp);

                GameObject reference = controller.GetComponent<Game>().GetPosition(predatorx, predatory);
                reference.GetComponent<Chessman>().SetXBoard(preyx);
                reference.GetComponent<Chessman>().SetYBoard(preyy);

                if(predatorx == actualpassantx && predatory == actualpassanty){
                    reference.GetComponent<Chessman>().SetXBoard(preyx);
                    reference.GetComponent<Chessman>().SetYBoard(preyy-1);
                }
                reference.GetComponent<Chessman>().SetCoords(); 
                reference.GetComponent<Chessman>().DestroyMovePlates();
                if(getPiece(predatorx, predatory)[1] == 'p' && preyy == 0)
                    playSound(recentmove + "promote");
                else
                    playSound(recentmove + "capture");
                controller.GetComponent<Game>().SetPosition(reference);
                controller.GetComponent<Game>().SetPositionEmpty(predatorx, predatory);
                controller.GetComponent<Game>().NextTurn();
            }
            if(anyattack == false || getPiece(predatorx, predatory)[0] == 'B'){
                playSound(recentmove);
                currentPlayer = "white";
                undo.Add(getStringPosition());
                redo.Clear();
                //actualpassantx = -1;
                //actualpassanty = -1;
                //enpassant = -1;
            }
        }   
        if(numPiece("Wking") == 0 && whitekings > 0 && moves > 0){
            whitekings = 0; 
        }
        else{
            whitekings = numPiece("Wking");
        }
        if(pawnpromote){
            promoteVisibility(true);
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "Select Promotion Piece:";
        }
        else if(setpiece){
            promoteVisibility(true);
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "Select Piece:";
        }
        else{
            promoteVisibility(false);
            if(!gameOver && mode != "levels")
                GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = false;
        }
        if(Input.GetKeyDown("left shift")){  //shift
            Debug.Log(getStringPosition());
        }
        if(Input.GetKeyDown("right shift")){  //shift
            randomPosition();
        }
        if(Input.GetKeyDown("return") && !input.GetComponent<InputField>().isFocused){  //enter
            NextTurn();
        }
        if(Input.GetKeyDown("space")){ //space
            //source.PlayOneShot(winclip); 
            Debug.Log(tutorialnum); 
            /*Debug.Log("undos");
            for(int i = 0; i < undo.Count; i++){
                Debug.Log(undo[i]);
            }
            Debug.Log("redos");
            for(int i = 0; i < redo.Count; i++){
                Debug.Log(redo[i]);
            }*/
            /*Debug.Log("allmoves");
            for(int i = 0; i < allmoves.Count; i++){
                Debug.Log(allmoves[i]);
            }
            Debug.Log("redomoves");
            for(int i = 0; i < redomoves.Count; i++){
                Debug.Log(redomoves[i]);
            }*/
            //setStringPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 16, 16);
           /* string s = "";
            for(int i = 0; i < medals.Length; i++){
                s += medals[i] + " ";
            } 
            Debug.Log(s);  /*
            for(int i = 0; i < playerWhite.Length; i++){
                Debug.Log(playerWhite[i].GetComponent<Chessman>().name);
            }   */     
        }
        if(Input.GetMouseButtonUp(0)){
            int[] location = MouseLocation();
            Debug.Log(location[0] + " " + location[1]); 
            Debug.Log(squareAttacked(location[0], location[1]));
        }
        if(Input.GetMouseButtonDown(0)){  //Mouse Down mouse down Mouse down 
            int[] location = MouseLocation();
            if(setpiece && PositionOnBoard(location[0], location[1]) && !pieceplatepositions[location[0],location[1]]){
                cancelPiecePlate();
            }

        }
    }
    public void OnMouseUp(){
        Debug.Log(Input.GetAxis("Mouse X") + " " + Input.GetAxis("Mouse Y"));
    }
    public void Undo(){
        //if(gameOver == true) return;f
        if(pawnpromote) return;
        gameOver = false;
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = false;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = false;
        if(undo.Count < 2) return;
        if(mode == "sandbox") currentPlayer = (currentPlayer == "white" ? "black" : "white");
        redo.Add(getStringPosition());
        undo.RemoveAt(undo.Count-1);
        string save = undo[undo.Count-1];
        setStringPosition(save);
        
        if((mode == "ingame" || mode == "sandbox")){
            redomoves.Add(allmoves[allmoves.Count-1]);
            allmoves.RemoveAt(allmoves.Count-1);
            if(allmoves.Count > 0 && allmoves[allmoves.Count-1].Length == 4){
                enpassant = allmoves[allmoves.Count-1][0] - 'a';
            } 
        }

        if(mode == "ingame" || mode == "sandbox") moves--;
        else if(undospent.Count > 0){
            redospent.Add(spent);
            if(undospent.Count == 1) spent = 0;
            else spent = undospent[undospent.Count-2];
            undospent.RemoveAt(undospent.Count-1);
        }
    }
    public void Redo(){
        if(redo.Count == 0) return;
        if(pawnpromote) return;
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = false;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = false;
        gameOver = false;
        string save = redo[redo.Count-1];
        setStringPosition(save);
        undo.Add(save);
        redo.RemoveAt(redo.Count-1);

        if((mode == "ingame" || mode == "sandbox")){
            allmoves.Add(redomoves[redomoves.Count-1]);
            redomoves.RemoveAt(redomoves.Count-1);
            if(allmoves.Count > 0 && allmoves[allmoves.Count-1].Length == 4){
                enpassant = allmoves[allmoves.Count-1][0] - 'a';
            } 
        }

        if(mode == "sandbox") currentPlayer = (currentPlayer == "white" ? "black" : "white");
        if(mode == "ingame" || mode == "sandbox") moves++;
        else if(redospent.Count > 0){
            spent = redospent[redospent.Count-1];
            undospent.Add(spent);
            redospent.RemoveAt(redospent.Count-1);
        }
    }
    public string getPiece(int x, int y){
        if(!PositionOnBoard(x,y)) return "Null";
        if(GetPosition(x,y) == null) return "Null";
        return GetPosition(x,y).GetComponent<Chessman>().name;
    }


    public void promoteVisibility(bool shown){
        int start = 0;
        if(mode == "setpiece") start = 1;
        if(shown){
            for(int i = start; i < start + 5; i++){
                if(i == 5 && budget - spent >= 1 || i == 1 && budget - spent >= 9 ||
                i == 2 && budget-spent >= 5 || i == 3 && budget-spent >= 3 ||
                i == 4 && budget-spent >= 3 || mode == "ingame")
                promotes[i].SetActive(shown);
            }
        }
        else{
            for(int i = 0; i < 6; i++){
                promotes[i].SetActive(shown);
            }
        }
    }
    public bool mouseOnPlate(){
        int[] location = MouseLocation();
        if(location[0] == -1 || location[1] == -1) return true;
        if(platepositions[location[0], location[1]] == 1) return true;
        return false;
    }
    public int[] MouseLocation(){
        Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            //Canvas canvas = this.GetComponent<Canvas>();
        Vector3 mousePos = Input.mousePosition;
        float f = canvas.scaleFactor;
        int boardx = -1;
        int boardy = -1;
        int cnt = 0;
        float x = mousePos.x/f;
        float y = mousePos.y/f;
        for(float i = 0.0f; i < 800.0f; i += 100.0f){
            if(i < x && x < i + 100.0f){
                boardx = cnt;
            }
            if(i+307.0f < y && y < i + 407.0f){
                boardy = cnt;
            }
            cnt++;
        }
        int[] location = {boardx,boardy};
        //Debug.Log(boardx + " " + boardy);
        return location;
    }
    public void nextTutorial(){
        tutorialnum++;
        tutorialChanged();
    }
    public void previousTutorial(){
        tutorialnum--;
        tutorialChanged();
    }
    public void tutorialChanged(){
        Text t = GameObject.FindGameObjectWithTag("TutorialText").GetComponent<Text>();
        string [,] tutorial = {{"Pieces move exactly the same as in normal chess. Try it out!","8/8/8/8/8/8/PPPPPPPP/RNBQKBNR"},
        {"Black pieces are lazy and won't move, unless it detects ANY white pieces in attacking range. Try pushing some pawns!","8/pppppppp/8/8/PPPPPPPP/8/8/8"},
        {"Remember, the objective is to capture the black king. Capture it!","k7/8/8/8/8/8/8/7R"},
        {"However, if black captures the white king, you lose! You'll have to undo or restart.","8/8/8/8/8/8/7q/K7"},
        {"Black will only capture one piece at a time. Move the white king to (sadly) watch your pawns fall, one by one.","7r/7P/7P/7P/7P/8/8/K7"},
        {"Nice job! You completed this tutorial. Just remember: capture the black king, and don't get your king captured! Press the home button to go back to the level select.","rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"},
        {"In cases where there are multiple capturable pieces that black can take, we need to know the piece priority rules.","8/8/8/8/8/8/8/8"},
        {"The following rules will tell us which piece black will CAPTURE, which we will call the JUICIEST white piece.", "8/8/8/8/8/8/8/8"},
        {"Black prioritizes capturing the highest-value piece, and then the furthest-advanced piece, and then the rightmost piece. Let's look at some examples.","8/8/8/8/8/8/8/8"},
        {"Black will first attempt to capture the highest-value piece. Pieces from least to most valuable: pawns, knights, bishops, rooks, queens, kings.", "8/8/8/8/8/8/8/8"},
        {"In this position, if it is black's turn, the black queen will capture the white queen instead of the white rook since a queen is more valuable. Move the white king to see what happens.","Q3q2R/8/8/8/8/8/8/7K"},
        {"If there are multiple capturable pieces that are the highest value, black will capture the piece furthest advanced. Move the white king to see what happens.","8/Nq6/NP6/6K1/8/8/8/8"},
        {"If there are multiple highest-value capturable pieces have advanced the furthest, black will capture the piece furthest to the right. Move the white king to see what happens.","R3q2R/4R3/8/8/8/8/8/7K"},
        {"Now, we know which piece black will capture. But what if there are multiple attackers of the juiciest piece?","8/8/8/8/8/8/8/8"},
        {"The following rules will tell us which piece black will MOVE to capture the piece (which we have already decided on from the last 3 rules).","8/8/8/8/8/8/8/8"},
        {"Black prioritizes moving the lowest-value piece, and then the topmost piece, and then the rightmost piece. Let's look at some examples.","8/8/8/8/8/8/8/8"},
        {"Of all the black pieces that can attack the juiciest white piece, black will first attempt to move its lowest-value piece. Move the white king to see what happens.","Rq6/r7/8/8/8/8/8/7K"},
        {"In this case, there are 2 such lowest-value pieces that attack the juciest white piece. So, black will use its upmost piece. Move the white king to see what happens.","Rr6/rq6/8/8/8/8/8/4K3"},
        {"In this case, the 2 lowest-value pieces that attack the juciest piece, also have the same y-position. So, black will use its rightmost piece. Move the white king to see what happens.","rRr5/8/8/8/8/8/8/7K"},
        {"Nice job! You completed this tutorial. Just remember: capture the black king, and don't get your king captured! Press the home button to go back to the level select.","rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"}};
        if(tutorialnum == 1){ 
            t.text = "Pieces move exactly the same as in normal chess. Try it out!";
            curposition = "8/8/8/8/8/8/PPPPPPPP/RNBQKBNR";
        }
        if(tutorialnum == 2){
            t.text = "Black pieces are lazy and won't move, unless it detects ANY white pieces in attacking range. Try pushing some pawns!";
            curposition = "8/pppppppp/8/8/PPPPPPPP/8/8/8";
        }
        if(tutorialnum == 3){
            t.text = "Remember, the objective is to capture the black king. Capture it!";
            curposition = "k7/8/8/8/8/8/8/7R";
        }
        if(tutorialnum == 4){
            t.text = "However, if black captures the white king, you lose! You'll have to undo or restart.";
            curposition = "8/8/8/8/8/8/7q/K7";
        }
        if(tutorialnum == 5){
            t.text = "Black will only capture one piece at a time. Move the white king to (sadly) watch your pawns fall, one by one.";
            curposition = "7r/7P/7P/7P/7P/8/8/K7";
        }
        if(tutorialnum == 6){ 
            medals[levelnum] = "gold";
            t.text = "Nice job! You completed this tutorial. Just remember: capture the black king, and don't get your king captured! Press the home button to go back to the level select.";
            curposition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        }
        if(tutorialnum == 7){
            t.text = "In cases where there are multiple capturable pieces that black can take, we need to know the piece priority rules.";
            curposition = "8/8/8/8/8/8/8/8";
        }
        if(tutorialnum == 8){
            t.text = "The following rules will tell us which piece black will CAPTURE, which we will call the JUICIEST white piece.";
            curposition = "8/8/8/8/8/8/8/8";
        }
        if(tutorialnum == 9){
            t.text = "Black prioritizes capturing the highest-value piece, and then the furthest-advanced piece, and then the rightmost piece. Let's look at some examples.";
            curposition = "8/8/8/8/8/8/8/8";
        }
        if(tutorialnum == 10){
            t.text = "Black will first attempt to capture the highest-value piece. Pieces from least to most valuable: pawns, knights, bishops, rooks, queens, kings.";
            curposition = "8/8/8/8/8/8/8/8";
        }
        if(tutorialnum == 11){
            t.text = "In this position, if it is black's turn, the black queen will capture the white queen instead of the white rook since a queen is more valuable. Move the white king to see what happens.";
            curposition = "Q3q2R/8/8/8/8/8/8/7K";
        }
        if(tutorialnum == 12){
            t.text = "If there are multiple capturable pieces that are the highest value, black will capture the piece furthest advanced. Move the white king to see what happens.";
            curposition = "8/Nq6/NP6/6K1/8/8/8/8";
        }
        if(tutorialnum == 13){
            t.text = "If there are multiple highest-value capturable pieces have advanced the furthest, black will capture the piece furthest to the right. Move the white king to see what happens.";
            curposition = "R3q2R/4R3/8/8/8/8/8/7K";
        }
        if(tutorialnum == 14){
            t.text = "Now, we know which piece black will capture. But what if there are multiple attackers of the juiciest piece?";
            curposition = "8/8/8/8/8/8/8/8";
        }
        if(tutorialnum == 15){
            t.text = "The following rules will tell us which piece black will MOVE to capture the piece (which we have already decided on from the last 3 rules).";
            curposition = "8/8/8/8/8/8/8/8";
        }
        if(tutorialnum == 16){
            t.text = "Black prioritizes moving the lowest-value piece, and then the topmost piece, and then the rightmost piece. Let's look at some examples.";
            curposition = "8/8/8/8/8/8/8/8";
        }
        if(tutorialnum == 17){
            t.text = "Of all the black pieces that can attack the juiciest white piece, black will first attempt to move its lowest-value piece. Move the white king to see what happens.";
            curposition = "Rq6/r7/8/8/8/8/8/7K";
        }
        if(tutorialnum == 18){
            t.text = "In this case, there are 2 such lowest-value pieces that attack the juciest white piece. So, black will use its upmost piece. Move the white king to see what happens.";
            curposition = "Rr6/rq6/8/8/8/8/8/4K3"; 
        }
        if(tutorialnum == 19){ 
            t.text = "In this case, the 2 lowest-value pieces that attack the juciest piece, also have the same y-position. So, black will use its rightmost piece. Move the white king to see what happens.";
            curposition = "rRr5/8/8/8/8/8/8/7K";
        }
        if(tutorialnum == 20){ 
            medals[levelnum] = "gold";
            t.text = "Nice job! You completed this tutorial. Just remember: capture the black king, and don't get your king captured! Press the home button to go back to the level select.";
            curposition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        }
        restart();
    }
    public void makeLevel(int i){
        tutorialnum = 0;
        levelnum = i;
        curposition = levels[i,1];
        metacurposition = curposition;
        curmovegoal1 = int.Parse(levels[i,2]);
        curmovegoal2 = int.Parse(levels[i,3]);
        mode = levels[i,4];
        title = levels[i,5];
        goal = levels[i,6];
        restart();
        if(mode == "setpiece") budget = int.Parse(levels[i,7]);
    }
    public bool pieceAttack(GameObject obj, bool justsquare = false, int cx = -1, int cy = -1){
        Chessman piece = obj.GetComponent<Chessman>();
        int x = piece.GetXBoard();
        int y = piece.GetYBoard();
        string name = piece.name.Substring(1);
        if(name == "rook"){
            return (attackLine(x,y,-1,0,obj,justsquare,cx,cy) | attackLine(x,y,0,-1,obj,justsquare,cx,cy) | attackLine(x,y,0,1,obj,justsquare,cx,cy) | attackLine(x,y,1,0,obj,justsquare,cx,cy));
        }
        if(name == "bishop"){
            return (attackLine(x,y,-1,1,obj,justsquare,cx,cy) | attackLine(x,y,-1,-1,obj,justsquare,cx,cy) | attackLine(x,y,1,1,obj,justsquare,cx,cy) | attackLine(x,y,1,-1,obj,justsquare,cx,cy));
        }
        if(name == "queen"){
            return (attackLine(x,y,-1,1,obj,justsquare,cx,cy) | attackLine(x,y,-1,-1,obj,justsquare,cx,cy) | attackLine(x,y,1,1,obj,justsquare,cx,cy) | attackLine(x,y,1,-1,obj,justsquare,cx,cy) |
                    attackLine(x,y,-1,0,obj,justsquare,cx,cy) | attackLine(x,y,0,-1,obj,justsquare,cx,cy) | attackLine(x,y,0,1,obj,justsquare,cx,cy) | attackLine(x,y,1,0,obj,justsquare,cx,cy));
        }
        if(name == "knight"){
            return (attackSquare(x+1,y+2,obj,justsquare,cx,cy) | attackSquare(x-1,y+2,obj,justsquare,cx,cy) | attackSquare(x+2,y+1,obj,justsquare,cx,cy) | attackSquare(x+2,y-1,obj,justsquare,cx,cy) |
            attackSquare(x+1,y-2,obj,justsquare,cx,cy) | attackSquare(x-1,y-2,obj,justsquare,cx,cy) | attackSquare(x-2,y+1,obj,justsquare,cx,cy) | attackSquare(x-2,y-1,obj,justsquare,cx,cy));
        }
        if(name == "king"){
            return (attackSquare(x,y+1,obj,justsquare,cx,cy) | attackSquare(x,y-1,obj,justsquare,cx,cy) | attackSquare(x+1,y+1,obj,justsquare,cx,cy) | attackSquare(x+1,y-1,obj,justsquare,cx,cy) |
            attackSquare(x+1,y,obj,justsquare,cx,cy) | attackSquare(x-1,y+1,obj,justsquare,cx,cy) | attackSquare(x-1,y,obj,justsquare,cx,cy) | attackSquare(x-1,y-1,obj,justsquare,cx,cy));
        }
        if(name == "pawn"){
            if(mode == "sandbox" && currentPlayer == "black")
                return (attackSquare(x+1, y+1, obj,justsquare,cx,cy) | attackSquare(x-1, y+1, obj,justsquare,cx,cy));
            return (attackSquare(x+1, y-1, obj,justsquare,cx,cy) | attackSquare(x-1, y-1, obj,justsquare,cx,cy));
        }
        return false;
    }
    public bool squareAttacked(int x, int y){
        bool anyattack = false;
        if(currentPlayer == "white"){
            for(int i = 0; i < playerBlack.Length; i++){
                if(playerBlack[i] == null) continue;
                anyattack = anyattack | (pieceAttack(playerBlack[i], true, x, y));
            }
        }
        else{
            for(int i = 0; i < playerWhite.Length; i++){
                if(playerWhite[i] == null) continue;
                anyattack = anyattack | (pieceAttack(playerWhite[i], true, x, y));
            }
        }
        return anyattack;
    }
    public int numPiece(string name){
        int cnt = 0;
        for(int x = 0; x < 8; x++){
            for(int y = 0; y < 8; y++){
                if(positions[x,y] != null){
                    if(positions[x,y].GetComponent<Chessman>().name == name)
                        cnt++;
                }

            }
        }
        return cnt;
    }

    public bool attackLine(int x, int y, int dx, int dy, GameObject obj, bool justsquare = false, int cx = -1, int cy = -1){
        x += dx;
        y += dy;
        if(!PositionOnBoard(x,y)) return false;
        if(justsquare){
            if(x == cx && y == cy) return true;
            if(positions[x,y] == null) return attackLine(x, y, dx, dy, obj, justsquare, cx, cy);
            return false;
        }

        if(positions[x,y] == null) return attackLine(x, y, dx, dy, obj);
        if(getPiece(x,y)[0] == 'B') return false;
        if(getPiece(x,y)[0] == 'W'){
            Debug.Log(getPiece(x,y));
            if((preyx == -1 || compare(GetPosition(preyx, preyy), GetPosition(x,y), "white"))){
                preyx = x;
                preyy = y;
                predatorx = obj.GetComponent<Chessman>().GetXBoard();
                predatory = obj.GetComponent<Chessman>().GetYBoard();
                actualpassantx = -1;
                actualpassanty = -1;
            }
            else if(preyx == x && preyy == y && compare(GetPosition(predatorx, predatory), obj, "black")){
                preyx = x;
                preyy = y;
                predatorx = obj.GetComponent<Chessman>().GetXBoard();
                predatory = obj.GetComponent<Chessman>().GetYBoard();
                actualpassantx = -1;
                actualpassanty = -1;
            }
            return true;
        } 
        return false;
    }

    public bool attackSquare(int x, int y, GameObject obj, bool justsquare = false, int cx = -1, int cy = -1){
        if(!PositionOnBoard(x,y)) return false;
        bool attackpassant = (x == enpassant) && y == 2 && PositionOnBoard(x, y+1) && positions[x,y+1] != null && obj.GetComponent<Chessman>().name[1] == 'p';
        if(justsquare) return cx == x && cy == y;
        if(positions[x,y] == null && !attackpassant) return false;
        if(attackpassant){
             y = y+1;
             actualpassantx = obj.GetComponent<Chessman>().GetXBoard();
             actualpassanty = obj.GetComponent<Chessman>().GetYBoard();
        }
        if(getPiece(x,y)[0] == 'W'){
            Debug.Log(getPiece(x,y));
            if((preyx == -1 || compare(GetPosition(preyx, preyy), GetPosition(x,y), "white"))){
                preyx = x;
                preyy = y;
                predatorx = obj.GetComponent<Chessman>().GetXBoard();
                predatory = obj.GetComponent<Chessman>().GetYBoard();
                if(!attackpassant){
                    actualpassantx = -1;
                    actualpassanty = -1;
                }
            }
            else if(preyx == x && preyy == y && compare(GetPosition(predatorx, predatory), obj, "black")){
                preyx = x;
                preyy = y;
                predatorx = obj.GetComponent<Chessman>().GetXBoard();
                predatory = obj.GetComponent<Chessman>().GetYBoard();
                if(!attackpassant){
                    actualpassantx = -1;
                    actualpassanty = -1;
                }
            }
            return true;
        }
        if(attackpassant){
            actualpassantx = -1;
            actualpassanty = -1;
        }
        return false;
    }

    public bool compare(GameObject obj, GameObject obj2, string piececolor){ //true if piece < piece2
        if(obj == null || obj2 == null) return false;
        Chessman piece = obj.GetComponent<Chessman>();
        Chessman piece2 = obj2.GetComponent<Chessman>();
        int x1 = piece.GetXBoard();
        int y1 = piece.GetYBoard();
        string name = piece.name;   
        int x2 = piece2.GetXBoard();
        int y2 = piece2.GetYBoard();
        string name2 = piece2.name;

        string [] order;
        order = new string[] {"Wpawn", "Wknight", "Wbishop", "Wrook", "Wqueen", "Wking"};
        if(piececolor == "black")
            order = new string[] {"Bpawn", "Bknight", "Bbishop", "Brook", "Bqueen", "Bking"};
        if(piececolor == "white" && indexOf(name, order) != indexOf(name2, order)) 
            return indexOf(name, order) < indexOf(name2, order);
        if(piececolor == "black" && indexOf(name, order) != indexOf(name2, order)) 
            return indexOf(name, order) > indexOf(name2, order);
        if(y2 != y1) return y1 < y2;
        return x1 < x2;
    }

    public int indexOf(string str, string [] arr){
        for(int i = 0; i < arr.Length; i++){
            if(arr[i] == str) return i;
        }
        return -1;
    }
    public void hideMainButtons(){
        restartbutton.SetActive(false);
        readybutton.SetActive(false);
        undobutton.SetActive(false);
        redobutton.SetActive(false);
    }
    public void showMainButtons(){
        restartbutton.SetActive(true);
        //readybutton.SetActive(true);
        if(mode != "setpiece"){
            undobutton.SetActive(true);
            redobutton.SetActive(true);
        }
    }
    public void ReadStringInput(string s){
        Debug.Log(s);
        setStringPosition(s);
        curposition = s;
        metacurposition = s;
        undo.Clear();
        redo.Clear();
        spent = 0; 
        undobutton.SetActive(true);
        redobutton.SetActive(true);
        undo.Add(curposition);
        goal = "points";
        if(metacurposition.Contains('x')) {
             mode = "setpiece";
             budget = 1000;
        }
        else
            mode = "ingame";
    }
    public void randomPosition(){
        char[,] randboard = new char [8, 8];
        char[] wpieces = {'Q', 'R', 'R', 'B', 'B', 'B', 'N', 'N', 'N', 'P','P','P','P','P','P','P','P','P'};
        char[] bpieces = {'q', 'r', 'r', 'b', 'b', 'b', 'n', 'n', 'n', 'p','p','p','p','p','p','p','p','p'}; 
        char[] bpieces2 = {'q', 'r', 'r', 'b', 'b', 'b', 'n', 'n', 'n'}; 
        for(int i = 7; i >= 5; i--){
            for(int j = 0; j < 8; j++){
                if(Random.Range(0, 16) == 0)
                    randboard[i, j] = wpieces[Random.Range(0, 18)];
                else if(Random.Range(0, 4) == 0)
                    randboard[i, j] = 'x';
                else
                    randboard[i, j] = '1';
                
                if(randboard[i,j] == 'P' && i == 7) randboard[i, j] = '1';
            }
        }
        for(int i = 2; i >= 0; i--){
            for(int j = 0; j < 8; j++){
                if(i != 0 && Random.Range(0, 2) == 0)
                    randboard[i, j] = bpieces[Random.Range(0, 18)];
                else if(i == 0 && Random.Range(0, 2) == 0)
                        randboard[i, j] = bpieces2[Random.Range(0, 9)];
                else
                    randboard[i, j] = '1';
            }
        }
        randboard[Random.Range(0, 1), Random.Range(0, 8)] = 'k';
        randboard[Random.Range(7, 8), Random.Range(0, 8)] = 'K';
        input.GetComponent<InputField>().text = renderStringPosition(print2D(randboard));
        ReadStringInput(print2D(randboard));
        restart();
    }
    public string print2D(char[,] arr){
        string s = "";
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                s += arr[i,j];
            }
            s += "/";
        }
        return s;
    }
    public int indexOf(string[] arr, string s){
        for(int i = 0; i < arr.Length; i++){
            if(s == arr[i]) return i;
        }
        return -1;
    }
    public void home(){
        
        makeLevel(0);
    }
    public void Winner(string playerWinner){
        if(mode == "levels") return;
        gameOver = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;

        restartbutton.SetActive(true);
        undobutton.SetActive(true);
        redobutton.SetActive(true);
        if(playerWinner == "black"){
            if(mode != "sandbox") GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "White King defeated! Try Again.";
            else GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "Black Wins!";
        }
        else{
            string medal = "";
            source.PlayOneShot(winclip); 
            if(goal == "moves"){
                if((moves+1) <= curmovegoal1)
                    medal = "gold";
                else if((moves+1) <= curmovegoal2)
                    medal = "silver";
                else 
                    medal = "bronze";
            }
            else{
                if(spent <= curmovegoal1)
                    medal = "gold";
                else if(spent <= curmovegoal2)
                    medal = "silver";
                else 
                    medal = "bronze";
            }
            string previous = medals[levelnum];
            string[] medalorder  = new string[4];
            medalorder[0] = "gray";
            medalorder[1] = "bronze";
            medalorder[2] = "silver";
            medalorder[3] = "gold";
            if(indexOf(medalorder, medal) > indexOf(medalorder, previous)){
                medals[levelnum] = medal;
                Menu.setMedals();
            }
            
            if(mode != "sandbox" && title != "Tutorial" && title != "Piece Priority Tutorial") GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "You Win! You get a " + medal + " medal.";
            else GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "White Wins!";
        }
        //GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
