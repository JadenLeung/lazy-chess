using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chessman : MonoBehaviour
{
    //References
    public GameObject controller;
    public GameObject movePlate;
    public GameObject mousePiece;
    public GameObject textmesh;

    //Positions
    public int xBoard = -1;
    public int yBoard = -1;
    private int zBoard = -1;
    public bool isTapped = false;

    //Variable to keep track of "black" or "white" player
    private string player;

    //References to all sprites chess piece can be
    public Sprite Bqueen, Bking, Bpawn, Bbishop, Bknight, Brook;
    public Sprite Wqueen, Wking, Wpawn, Wbishop, Wknight, Wrook;
    private int tapping = 0;  

    public void Start(){
        DestroyMousePiece(); 
    }
    public void Activate(){
        controller = GameObject.FindGameObjectWithTag("GameController");


        //Takes the instantiated location and adjusts the transform
        SetCoords();

        switch(this.name){
            case "Bqueen": this.GetComponent<SpriteRenderer>().sprite = Bqueen; player = "black"; break;
            case "Bknight": this.GetComponent<SpriteRenderer>().sprite = Bknight; player = "black"; break;
            case "Bbishop": this.GetComponent<SpriteRenderer>().sprite = Bbishop; player = "black"; break;
            case "Bking": this.GetComponent<SpriteRenderer>().sprite = Bking; player = "black"; break;
            case "Bpawn": this.GetComponent<SpriteRenderer>().sprite = Bpawn; player = "black"; break;
            case "Brook": this.GetComponent<SpriteRenderer>().sprite = Brook; player = "black"; break;

            case "Wqueen": this.GetComponent<SpriteRenderer>().sprite = Wqueen; player = "white"; break;
            case "Wknight": this.GetComponent<SpriteRenderer>().sprite = Wknight; player = "white"; break;
            case "Wbishop": this.GetComponent<SpriteRenderer>().sprite = Wbishop; player = "white"; break;
            case "Wking": this.GetComponent<SpriteRenderer>().sprite = Wking; player = "white"; break;
            case "Wpawn": this.GetComponent<SpriteRenderer>().sprite = Wpawn; player = "white"; break;
            case "Wrook": this.GetComponent<SpriteRenderer>().sprite = Wrook; player = "white"; break;
        }
    } 
    public void setSprite(string spritename){
        if(spritename == "Wqueen")
            this.GetComponent<SpriteRenderer>().sprite = Wqueen;
        if(spritename == "Wrook")
            this.GetComponent<SpriteRenderer>().sprite = Wrook;
        if(spritename == "Wbishop")
            this.GetComponent<SpriteRenderer>().sprite = Wbishop;
        if(spritename == "Wknight")
            this.GetComponent<SpriteRenderer>().sprite = Wknight;
        if(spritename == "Wking")
            this.GetComponent<SpriteRenderer>().sprite = Wking;
        if(spritename == "Bqueen")
            this.GetComponent<SpriteRenderer>().sprite = Bqueen;
        if(spritename == "Brook")
            this.GetComponent<SpriteRenderer>().sprite = Brook;
        if(spritename == "Bbishop")
            this.GetComponent<SpriteRenderer>().sprite = Bbishop;
        if(spritename == "Bknight")
            this.GetComponent<SpriteRenderer>().sprite = Bknight;
        if(spritename == "Bking")
            this.GetComponent<SpriteRenderer>().sprite = Bking;
    }
    public void SetCoords(){
        float x = xBoard;
        float y = yBoard;

        x *= 0.7f;
        y *= 0.7f;

        x += -2.45f;
        y += -2.45f;

        this.transform.position = new Vector3(x, y, zBoard);
    }
    public int GetXBoard(){
        return xBoard;
    }

    public int GetYBoard(){
        return yBoard;
    }

    public void SetXBoard(int x){
        xBoard = x;
    }

    public void SetYBoard(int y){
        yBoard = y;
    }
    public void SetZBoard(int z){
        zBoard = z;
    }
    public void OnMouseDown(){ 
        Debug.Log("Down");
        clicked();
    } 
    public void clicked(){
        Debug.Log("Clicked");
        if(controller.GetComponent<Game>().pawnpromote || controller.GetComponent<Game>().mode == "setpiece"){
            return;
        }
        if(!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player && !isTapped){
            DestroyMovePlates();
            InitiateMovePlates();
            tapping = 1;
            isTapped = true;
        }
        if(player == controller.GetComponent<Game>().currentPlayer){
            MousePieceSpawn();
            zBoard = 4;
            SetCoords();
        }
    }
    public void OnMouseUp(){ 
        DestroyMousePiece();
        zBoard = -1;
        SetCoords();

        if(controller.GetComponent<Game>().pawnpromote || controller.GetComponent<Game>().mode == "setpiece"){
            return;
        }
        if(!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player && tapping == 3){
            DestroyMovePlates();
        }
    } 
    public void DestroyMovePlates(bool allplates = false){
        controller.GetComponent<Game>().platepositions = new int [8, 8];
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate"); 
        for(int i = 0; i < movePlates.Length; i++){
            if(movePlates[i] != null && movePlates[i].GetComponent<MovePlate>().GetReference() != null){
                movePlates[i].GetComponent<MovePlate>().GetReference().GetComponent<Chessman>().isTapped = false;
                movePlates[i].GetComponent<MovePlate>().GetReference().GetComponent<Chessman>().tapping = 0;
            }
            if((!movePlates[i].GetComponent<MovePlate>().pieceplate && !movePlates[i].GetComponent<MovePlate>().levelplate) || allplates)
                Destroy(movePlates[i]); 
        }
        isTapped = false;
        tapping = 0;
        //("Destroying!" + tapping + " " + xBoard + " " + yBoard);
    }

    public void DestroyMousePiece(){
        GameObject[] mousePieces = GameObject.FindGameObjectsWithTag("MousePiece");
        for(int i = 0; i < mousePieces.Length; i++){
            Destroy(mousePieces[i]);
        } 
    }

    public void InitiateMovePlates(){
        controller.GetComponent<Game>().platepositions = new int [8, 8];
        controller.GetComponent<Game>().platepositions[xBoard, yBoard] = 1;
        switch(this.name){
            case "Bqueen": 
            case "Wqueen":
                LineMovePlate(1,0);
                LineMovePlate(0,1);
                LineMovePlate(1,1);
                LineMovePlate(-1,0);
                LineMovePlate(0,-1);
                LineMovePlate(-1,-1);
                LineMovePlate(-1,1);
                LineMovePlate(1,-1);
                break;
            case "Bknight":
            case "Wknight":
                LMovePlate();
                break;
            case "Bbishop":
            case "Wbishop":
                LineMovePlate(1,1);
                LineMovePlate(-1,1);
                LineMovePlate(1,-1);
                LineMovePlate(-1,-1);
                break;
            case "Bking":
            case "Wking":
                SurroundMovePlate();
                break;
            case "Brook":
            case "Wrook":
                LineMovePlate(0,1);
                LineMovePlate(1,0);
                LineMovePlate(-1,0);
                LineMovePlate(0,-1);
                break;
            case "Bpawn":
                if (yBoard == 6)
                {
                    PawnMovePlate(xBoard, yBoard - 1, true);
                    if(controller.GetComponent<Game>().GetPosition(xBoard,yBoard-1) == null)
                        PawnMovePlate(xBoard, yBoard - 2, false);
                }
                else
                {
                    PawnMovePlate(xBoard, yBoard - 1, true);
                }
                Game sc = controller.GetComponent<Game>();
                if(yBoard == 3 && sc.enpassant != -1 && (xBoard == sc.enpassant+1 || xBoard == sc.enpassant-1)){
                    PawnMovePlate(sc.enpassant, yBoard - 1, true, true);
                } 
                break;
            case "Wpawn":
                if (yBoard == 1)
                {
                    PawnMovePlate(xBoard, yBoard + 1, true);
                    if(controller.GetComponent<Game>().GetPosition(xBoard,yBoard+1) == null)
                        PawnMovePlate(xBoard, yBoard + 2, false);
                }
                else
                {
                    PawnMovePlate(xBoard, yBoard + 1, true);
                }

                sc = controller.GetComponent<Game>();
                if(yBoard == 4 && sc.enpassant != -1 && (xBoard == sc.enpassant+1 || xBoard == sc.enpassant-1)){
                    PawnMovePlate(sc.enpassant, yBoard + 1, true, true);
                } 

                break;
                
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement){
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null){ //If valid on board and nothing blocking it
            MovePlateSpawn(x,y);
            x += xIncrement;
            y += yIncrement;
        }

        if(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y).GetComponent<Chessman>().player != player){
            MovePlateAttackSpawn(x, y);
        }
    }
    public void LMovePlate(){
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard  - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    } 

    public void SurroundMovePlate(){
        Game sc = controller.GetComponent<Game>();
        PointMovePlate(xBoard, yBoard + 1, true);
        PointMovePlate(xBoard, yBoard - 1, true);
        PointMovePlate(xBoard + 1, yBoard + 1, true);
        PointMovePlate(xBoard + 1, yBoard - 1, true);
        PointMovePlate(xBoard + 1, yBoard, true);
        PointMovePlate(xBoard - 1, yBoard + 1, true);
        PointMovePlate(xBoard - 1, yBoard - 1, true);
        PointMovePlate(xBoard - 1, yBoard, true);

        if(xBoard == 4 && yBoard == 0 && this.name[0] == 'W' && sc.GetPosition(xBoard+1, yBoard) == null && sc.getPiece(7,0) == "Wrook"
        && !sc.squareAttacked(6,0) && !sc.squareAttacked(5,0) && !sc.squareAttacked(4,0) && sc.Nevermoved('K',4,0) && sc.Nevermoved('R',7,0)){ //castling
            PointMovePlate(xBoard + 2, yBoard, true, 1);
        }
        if(xBoard == 4 && yBoard == 0 && this.name[0] == 'W' && sc.GetPosition(1,0) == null && sc.GetPosition(2,0) == null && sc.GetPosition(3,0) == null && sc.getPiece(0,0) == "Wrook" && !sc.squareAttacked(2,0) && !sc.squareAttacked(3,0) && !sc.squareAttacked(4,0)
        && sc.Nevermoved('K',4,0) && sc.Nevermoved('R',0,0)){ //castling
            PointMovePlate(xBoard - 2, yBoard, true, 2);
        }
        if(xBoard == 4 && yBoard == 7 && this.name[0] == 'B' && sc.GetPosition(xBoard+1, yBoard) == null && sc.getPiece(7,7) == "Brook"
        && !sc.squareAttacked(6,7) && !sc.squareAttacked(5,7) && !sc.squareAttacked(4,7) && sc.Nevermoved('k',4,7) && sc.Nevermoved('r',7,7)){ //castling
            PointMovePlate(xBoard + 2, yBoard, true, 3);
        }
        if(xBoard == 4 && yBoard == 7 && this.name[0] == 'B' && sc.GetPosition(1,7) == null && sc.GetPosition(2,7) == null && sc.GetPosition(3,7) == null && sc.getPiece(0,7) == "Brook" && !sc.squareAttacked(2,7) && !sc.squareAttacked(3,7) && !sc.squareAttacked(4,7)
        && sc.Nevermoved('k',4,7) && sc.Nevermoved('r',0,7)){ //castling
            PointMovePlate(xBoard - 2, yBoard, true, 4);
        }
    }

    public void PointMovePlate(int x, int y, bool isking = false, int castle = 0){
        Game sc = controller.GetComponent<Game>();
        //if(isking && sc.squareAttacked(x,y)) return; 
        if(sc.PositionOnBoard(x,y)){
            GameObject cp = sc.GetPosition(x,y);
            if(cp == null){
                MovePlateSpawn(x,y,castle);
            }else if(cp.GetComponent<Chessman>().player != player){
                MovePlateAttackSpawn(x,y);
            }
        }
    }

    public void PawnMovePlate(int x, int y, bool shouldAttack, bool ispassant = false)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if(ispassant){
                MovePlateAttackSpawn(x, y, true);
                return;
            }
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (shouldAttack && sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (shouldAttack && sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }
    public void MousePieceSpawn(){
        //mousePiece = GameObject.FindGameObjectWithTag("MousePiece"); 
        GameObject mp = Instantiate(mousePiece, new Vector3(0, 0, -3.0f), Quaternion.identity);
        mp.GetComponent<MousePiece>().name = this.name;
        mp.GetComponent<MousePiece>().Activate();
        //mp.GetComponent<SpriteRenderer>().sortingOrder = 1;
        //(mp.GetComponent<MousePiece>().name);
        //mp.GetComponent<MousePiece>().setSprite("WQueen");
    }
    
    public void MovePlateSpawn(int matrixX, int matrixY, int castle = 0)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        controller.GetComponent<Game>().platepositions[matrixX, matrixY] = 1;

        //Adjust by variable offset
        x *= 0.7f;
        y *= 0.7f;

        //Add constants (pos 0,0)
        x += -2.45f;
        y += -2.45f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        //mp.GetComponent<SpriteRenderer>().sortingOrder = 0;
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.castleplate = castle;
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY, bool ispassant = false)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        controller.GetComponent<Game>().platepositions[matrixX, matrixY] = 1;

        //Adjust by variable offset
        x *= 0.7f;
        y *= 0.7f;

        //Add constants (pos 0,0)
        x += -2.45f;
        y += -2.45f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.passantplate = ispassant;
    }
    void Update()
    {   
        if(!controller.GetComponent<Game>().pawnpromote && !(controller.GetComponent<Game>().mode == "setpiece")){  
            if(Input.GetMouseButtonDown(1) && isTapped) DestroyMovePlates();
            if(Input.GetMouseButtonDown(0) && tapping == 2){
                tapping = 3;
                //("tapping3");
            } 
            if(Input.GetMouseButtonUp(0) && tapping == 1){
                tapping = 2;
               // Debug.Log("tapping2");
            } 
            if(!controller.GetComponent<Game>().mouseOnPlate() && Input.GetMouseButtonUp(0) && isTapped)
                DestroyMovePlates(); 
            switch (this.name){
                case "Bpawn": 
                    if (yBoard == 0 ){

                    GameObject cp = controller.GetComponent<Game>().GetPosition(xBoard, yBoard);
                    cp.name = "Bqueen";
                    this.GetComponent<SpriteRenderer>().sprite = Bqueen; player = "black";

                    }
                    break;
            }
        }
        if(Input.GetKeyDown("space")){ //space
            //DestroyMousePiece(); 
        }
        if(Input.GetKeyDown("right shift")){ //shift
            MousePieceSpawn();
        }
    }
    public int[] MouseLocation(float x, float y){
        int boardx = -1;
        int boardy = -1;
        int cnt = 0;
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
}
