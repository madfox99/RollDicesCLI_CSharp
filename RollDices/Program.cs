/* DON-CODE */
using Terminal.Gui;
using NStack;
using System;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections.Generic;

/* Variables */
string consoleOldTitle = null;
bool windowC_warn = false;
String[] playerNames = new String[2];
String[] diceFace = new string[6]{"┌─────────────┐\n│             │\n│             │\n│      o      │\n│             │\n│             │\n└─────────────┘","┌─────────────┐\n│             │\n│             │\n│    o   o    │\n│             │\n│             │\n└─────────────┘","┌─────────────┐\n│             │\n│   o         │\n│      o      │\n│         o   │\n│             │\n└─────────────┘","┌─────────────┐\n│             │\n│    o   o    │\n│             │\n│    o   o    │\n│             │\n└─────────────┘","┌─────────────┐\n│             │\n│    o   o    │\n│      o      │\n│    o   o    │\n│             │\n└─────────────┘","┌─────────────┐\n│             │\n│   o  o  o   │\n│             │\n│   o  o  o   │\n│             │\n└─────────────┘"};
String[] diceAnimation = new string[6]{"    ___________\n   /\\   o   o  \\\n  / o\\     o    \\\n /    \\   o   o  \\\n/   o  \\__________\\\n\\      /          /\n \\  o /   o o o  /\n  \\  /   o o o  /\n   \\/__________/","    __________\n   / o    o   /\\\n  /    o     /  \\\n /  o    o  /  o \\\n/__________/     o\\\n\\          \\ o    /\n \\  o o o   \\  o /\n  \\  o o o   \\  /\n   \\__________\\/","    ___________\n   /\\          \\\n  / o\\     o    \\\n /    \\          \\\n/ o o o\\__________\\\n\\      /          /\n \\  o /   o   o  /\n  \\  /   o   o  /\n   \\/__________/","    __________\n   /          /\\\n  /    o     /  \\\n /          /    \\\n/__________/    o \\\n\\          \\  o   /\n \\  o  o    \\    /\n  \\   o  o   \\  /\n   \\__________\\/","    ___________\n   /\\   o      \\\n  /  \\     o    \\\n /    \\       o  \\\n/  o   \\__________\\\n\\      /          /\n \\    /   o      /\n  \\  /      o   /\n   \\/__________/","    __________\n   / o        /\\\n  /    o     /  \\\n /       o  /  o \\\n/__________/  o o \\\n\\          \\ o o  /\n \\   o      \\ o  /\n  \\     o    \\  /\n   \\__________\\/"};
int gamePlayerRoute = 0;
bool gameStart = false;
bool gameRunning = true;
int[] player_score=new int[2]{0,0};
var dir = @"C:\\Users\\"+Environment.UserName+"\\Documents\\RollDices";
var file = dir+"\\myOutput.csv";

/* Maximize the console window (WINDOWS ONLY) */
try{
	[DllImport("kernel32.dll", ExactSpelling = true)]
	static extern IntPtr GetConsoleWindow();
	IntPtr ThisConsole = GetConsoleWindow();
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
	const int MAXIMIZE = 3;
	ShowWindow(ThisConsole, MAXIMIZE);
}catch(Exception ex){
	// Ignore
}

/* Change terminal title (WINDOWS ONLY) */
try{
	consoleOldTitle=Console.Title;
	Console.Title="Roll Dices v0.0.1";
}catch(Exception ex){
	// Ignore
}

/* Create RollDices directory if not exits (WINDOWS ONLY) */
try{
	if (!Directory.Exists(dir)){
		Directory.CreateDirectory(dir);
	}
}catch(Exception ex){
	// Ignore
}
createCSVFile();

Application.Init();
var top = Application.Top;		
var win = new Window(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), "");
win.ColorScheme = Colors.TopLevel;
top.Add(win);

Rect page = new Rect((Console.BufferWidth-51)/2, (Console.WindowHeight-24)/2, 51, 24);
Window windowA = new Window(page, "");
Window windowC = new Window(page, "");
Window windowD = new Window(page, "");
Window windowE = new Window(page, "");
Window windowF = new Window(page, "");
Window windowG = new Window(page, "");

/* Window A */
var windowA_logoText = new Label(" ___     _ _   ___  _\n| _ \\___| | | |   \\(_)__ ___ ___\n|   / _ \\ | | | |) | / _/ -_|_-<\n|_|_\\___/_|_| |___/|_\\__\\___/__/",true){
	X = 8,
	Y = 2
};
var windowA_startGameButton = new Button("Start Game"){
	X = Pos.Left(windowA_logoText) + 8,
	Y = Pos.Top(windowA_logoText) + 8	
};
var windowA_scoreButton = new Button("Score  board"){
	X = Pos.Left(windowA_startGameButton) - 1,
	Y = Pos.Top(windowA_startGameButton) + 2
};
var windowA_howToButton = new Button("  How To  "){
	X = Pos.Left(windowA_startGameButton),
	Y = Pos.Top(windowA_scoreButton) + 2,
};
var windowA_quitButton = new Button("   Quit   "){
	X = Pos.Left(windowA_startGameButton),
	Y = Pos.Top(windowA_howToButton) + 2,
};
var windowA_author = new Label("DON-CODE",true){
	X = 1,
	Y = 21
};
var windowA_version = new Label("v0.0.1",true){
	X = Pos.Left(windowA_author) + 41,
	Y = Pos.Top(windowA_author)
};

/* Window C */
var windowC_title = new Label("Enter Player Names\n══════════════════",true){
	X = 16,
	Y = 1
};
var windowC_player01Label = new Label("Player 01: "){
	X = 4,
	Y = Pos.Top(windowC_title) + 6
};
var windowC_player01TextField = new TextField(""){
	X = Pos.Right(windowC_player01Label) + 1,
	Y = Pos.Top(windowC_player01Label),
	Width = 30
};
var windowC_player02Label = new Label("Player 02: "){
	X = Pos.Left(windowC_player01Label),
	Y = Pos.Top(windowC_player01Label) + 3
};
var windowC_player02TextField = new TextField(""){
	X = Pos.Right(windowC_player02Label) + 1,
	Y = Pos.Top(windowC_player02Label),
	Width = 30
};
var windowC_messageLabel = new Label("(Please use maximum 20 letters)"){
	X = Pos.Left(windowC_player01Label) + 5,
	Y = Pos.Top(windowC_player01Label) + 7,
};
var windowC_warning = new Label("Please enter players' names",true){
	X = Pos.Left(windowC_messageLabel) + 2,
	Y = Pos.Top(windowC_messageLabel) + 3,
};
windowC_warning.ColorScheme = Colors.Error;
var windowC_backToMainButton = new Button("Back"){
	X = 1,
	Y = 21
};
var windowC_okButton = new Button("Ok"){
	X = Pos.Left(windowC_backToMainButton) + 41,
	Y = Pos.Top(windowC_backToMainButton)
};

/* Window D */
var windowD_player01ButtonFrame = new Label("┌────────────────────┐\n│                    │\n│                    │\n│                    │\n└────────────────────┘",true){
	X = 1,
	Y = 17
};
var windowD_player01Button = new Button("Play"){
	X = Pos.Left(windowD_player01ButtonFrame) + 1,
	Y = Pos.Top(windowD_player01ButtonFrame) + 1,
	Width = 20,
	Height = 3
};
var windowD_player02ButtonFrame = new Label("┌────────────────────┐\n│                    │\n│                    │\n│                    │\n└────────────────────┘",true){
	X = Pos.Right(windowD_player01ButtonFrame) + 2,
	Y = Pos.Top(windowD_player01ButtonFrame)
};
var windowD_player02Button = new Button("Play"){
	X = Pos.Right(windowD_player01Button) + 4,
	Y = Pos.Top(windowD_player01ButtonFrame) + 1,
	Width = 20,
	Height = 3
};
var windowD_progressBar1 = new Label("|--------------------|",true){
	X = 1,
	Y = 0
};
var windowD_progressBar2 = new Label("|--------------------|",true){
	X = Pos.Left(windowD_progressBar1) + 25,
	Y = Pos.Top(windowD_progressBar1)
};
var windowD_messageLabel = new Label("┌──────────────────────────┐\n│ Who wants to play first? │\n│                          │\n│                          │\n│                          │\n│                          │\n│                          │\n└──────────────────────────┘",true){
	X = Pos.Left(windowD_progressBar1) + 10,
	Y = Pos.Top(windowD_progressBar1) + 8
};
windowD_messageLabel.ColorScheme = Colors.Dialog;
var windowD_playButtonFrame = new Label("┌─────────────────────────────────────────┐\n│                                         │\n└─────────────────────────────────────────┘",true){
	X = Pos.Left(windowD_progressBar1) + 2,
	Y = Pos.Top(windowD_progressBar1) + 19
};
var windowD_playButton = new Button("Play"){
	X = Pos.Left(windowD_playButtonFrame) + 1,
	Y = Pos.Top(windowD_playButtonFrame) + 1,
	Width = 41
};
var windowD_messagePlayer01 = new Button();
var windowD_messagePlayer02 = new Button();
var dice1 = new Label ();
var dice2 = new Label ();
var windowD_playerTurnMessage = new Label();
var windowD_playerNotifyMessage = new Label();
var anim1 = new Label ();
var anim2 = new Label ();

/* Window E */
var windowE_picture = new Label();
var windowE_title = new Label();
var windowE_titleMessage = new Label();
var windowE_mainMenu = new Button("Main Menu"){
	X = 1,
	Y = 21
};
var windowE_newGame = new Button("New Game"){
	X = Pos.Right(windowE_mainMenu) + 6,
	Y = Pos.Top(windowE_mainMenu)
};
var windowE_quit = new Button("Quit"){
	X = Pos.Left(windowE_mainMenu) + 39,
	Y = Pos.Top(windowE_mainMenu)
};

/* Window F */
var windowF_title = new Label("Score Board\n═══════════",true){
	X = 19,
	Y = 1
};
var windowF_tableView = new TableView () {
    X = 3,
    Y = 3,
    Width = 43,
    Height = 17,
};
var windowF_backToMainButton = new Button("Back"){
	X = 1,
	Y = 21
};
var windowF_resetButton = new Button("Reset"){
	X = Pos.Left(windowF_backToMainButton) + 38,
	Y = Pos.Top(windowF_backToMainButton)
};

/* Window G */
var windowG_title = new Label("How To Guid\n═══════════",true){
	X = 19,
	Y = 1
};
var windowG_guid = new Label("■ This game is a two player game, because of\n  that after you start game you should enter\n  two players' name in given box.\n■ Then you two must decide who play first and\n  after that game will start.\n■ There's simple game rules in this game,\n  [1] Each player get chance to roll two\n      dices one after other.\n  [2] If you got snake eyes(two ones), you\n      lost all your score.\n  [3] if you got two same numbers, you have\n      another chance to roll dices.",true){
	X = 1,
	Y = 4
};
var windowG_backToMainButton = new Button("Back"){
	X = 1,
	Y = 21
};

/* Functions */
void setScore(int player){
	string scoreDisplay;
	int newScore = player_score[player];
	if(player==0){
		windowD.Remove(
			windowD_progressBar1
		);
	}else if(player==1){
		windowD.Remove(
			windowD_progressBar2
		);
	}	
	if(0 <= newScore && newScore <= 5){
		scoreDisplay = "|■-------------------|";
	}else if(5 <= newScore && newScore <= 10){
		scoreDisplay = "|■■------------------|";
	}else if(10 <= newScore && newScore <= 15){
		scoreDisplay = "|■■■-----------------|";
	}else if(15 <= newScore && newScore <= 20){
		scoreDisplay = "|■■■■----------------|";
	}else if(20 <= newScore && newScore <= 25){
		scoreDisplay = "|■■■■■---------------|";
	}else if(25 <= newScore && newScore <= 30){
		scoreDisplay = "|■■■■■■--------------|";
	}else if(30 <= newScore && newScore <= 35){
		scoreDisplay = "|■■■■■■■-------------|";
	}else if(35 <= newScore && newScore <= 40){
		scoreDisplay = "|■■■■■■■■------------|";
	}else if(40 <= newScore && newScore <= 45){
		scoreDisplay = "|■■■■■■■■■-----------|";
	}else if(45 <= newScore && newScore <= 50){
		scoreDisplay = "|■■■■■■■■■■----------|";
	}else if(50 <= newScore && newScore <= 55){
		scoreDisplay = "|■■■■■■■■■■■---------|";
	}else if(55 <= newScore && newScore <= 60){
		scoreDisplay = "|■■■■■■■■■■■■--------|";
	}else if(60 <= newScore && newScore <= 65){
		scoreDisplay = "|■■■■■■■■■■■■■-------|";
	}else if(65 <= newScore && newScore <= 70){
		scoreDisplay = "|■■■■■■■■■■■■■■------|";
	}else if(70 <= newScore && newScore <= 75){
		scoreDisplay = "|■■■■■■■■■■■■■■■-----|";
	}else if(75 <= newScore && newScore <= 80){
		scoreDisplay = "|■■■■■■■■■■■■■■■■----|";
	}else if(80 <= newScore && newScore <= 85){
		scoreDisplay = "|■■■■■■■■■■■■■■■■■---|";
	}else if(85 <= newScore && newScore <= 90){
		scoreDisplay = "|■■■■■■■■■■■■■■■■■■--|";
	}else if(90 <= newScore && newScore <= 95){
		scoreDisplay = "|■■■■■■■■■■■■■■■■■■■-|";
	}else if(95 <= newScore && newScore < 100){
		scoreDisplay = "|■■■■■■■■■■■■■■■■■■■■|";
	}else if(newScore >= 100){
		scoreDisplay = "|--------------------|";
		windowD_progressBar1 = new Label(scoreDisplay, true){
			X = 1,
			Y = 0
		};
		windowD_progressBar2 = new Label(scoreDisplay, true){
			X = 26,
			Y = 0
		};
		windowD.Add(
			windowD_progressBar1,
			windowD_progressBar2
		);
		windowE_title = new Label("Congratulations,", true){
			X = 17,
			Y = 2
		};
		windowE_titleMessage = new Label(playerNames[player]+" win...!", true){
			X = Pos.Left(windowE_title) + 1,
			Y = 3
		};
		windowE_picture = new Label("                        .''.\n               *''*    :_\\/_:     .\n      .    .:.*_\\/_*   : /\\ :  .'.:.'.\n    _\\(/_  ':'* /\\ *  : '..'.  -=:o:=-\n   . /)\\*''*  .┴.* '.\\'/.'_\\(/_'.':'.'\n      '*_\\/_* │ │  -= o =- /)\\    '  *\n       * /\\ * │'│  .'/.\\'.  '┌───┐\n      ┌─*┐.* ┌┘'│     :  ┌┐  │.··│  ┌──┐\n   ┌──│≡≡└─┐ │''│     ┌──┘│  │|··│┌─│··│\n┌──│°°│≡≡≡≡│┌┘  '-__  │···│──│···││·│··│\n│··│°°│≡≡≡≡││·······│ │···│°°│···││·│··│\n│··│°°!≡≡≡≡│|·······!-'   │°°│···│┘·│··│_\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯ ¯\n", true){
			X = 4,
			Y = 6
		};
		windowE.Add(
			windowE_title,
			windowE_titleMessage,
			windowE_picture
		);
		windowReplace("windowE");
		addDataToFile(playerNames[player], player_score[player].ToString(),"WIN");
	}else{
		player_score[gamePlayerRoute] = 0;
		scoreDisplay = "|--------------------|";		
	}	
	if(player==0){
		windowD_progressBar1 = new Label(scoreDisplay, true){
			X = 1,
			Y = 0
		};
		windowD.Add(
			windowD_progressBar1
		);
	}else if(player==1){
		windowD_progressBar2 = new Label(scoreDisplay, true){
			X = 26,
			Y = 0
		};
		windowD.Add(
			windowD_progressBar2
		);
	}	
}

bool Quit(){
	var n = MessageBox.Query(50, 7, "Quit Game", "Do you want to quit the game?", "Yes", "No");
	return n == 0;
}

void setRandValueInToDiceFace(){
	Random r = new Random();
	int[] values = new int[2]{r.Next(0, 6), r.Next(0, 6)};
	dice1 = new Label (diceFace[values[0]]) { X = 6, Y = 8 };
	dice2 = new Label (diceFace[values[1]]) { X = Pos.Left(dice1) + 22, Y = Pos.Top(dice1)};	
	windowD.Add(
		dice1,
		dice2
	);
	gameRunning = true;
	if(values[0]==values[1]){
		if(values[0] == 0){
			player_score[gamePlayerRoute] = -1;
			setScore(gamePlayerRoute);			
			windowD.Remove(
				windowD_playerTurnMessage
			);
			nextPlayer();
			playMessage(playerNames[gamePlayerRoute]);
			nextPlayer();			
			playMessageNotify(playerNames[gamePlayerRoute] + ", you got snake eyes... :(");	
			nextPlayer();
		}else{
			player_score[gamePlayerRoute] = player_score[gamePlayerRoute] + values[0] + values[1] + 2;
			setScore(gamePlayerRoute);
			windowD.Remove(
				windowD_playerTurnMessage
			);
			playMessage(playerNames[gamePlayerRoute]);
			playMessageNotify("Hey, you got a another chance...");
		}
		
	}else{
		player_score[gamePlayerRoute] = player_score[gamePlayerRoute] + values[0] + values[1];
		setScore(gamePlayerRoute);
		windowD.Remove(
			windowD_playerTurnMessage
		);
		nextPlayer();
		playMessage(playerNames[gamePlayerRoute]);
	}
}

/* Create score.csv file if not exists (WINDOWS ONLY) */
void createCSVFile(){
	try{
		if (!File.Exists(file)){
			File.WriteAllText(file, "Name,Score,Status");
		}
	}catch(Exception ex){
		// Ignore
	}
}

void windowReplace(string windowName){
	var subView = win.Subviews[0];
	subView.RemoveAll();
	switch(windowName){
		case "windowA":
			win.Add(windowA);
			windowA.FocusFirst();
			windowA.LayoutSubviews();
			break;
		case "windowC":
			win.Add(windowC);
			windowC.FocusFirst();
			windowC.LayoutSubviews();
			break;
		case "windowD":
			win.Add(windowD);
			windowD.FocusFirst();
			windowD.LayoutSubviews();
			break;
		case "windowE":
			win.Add(windowE);
			windowE.FocusFirst();
			windowE.LayoutSubviews();
			break;
		case "windowF":
			win.Add(windowF);
			windowF.FocusFirst();
			windowF.LayoutSubviews();
			break;
		case "windowG":
			win.Add(windowG);
			windowG.FocusFirst();
			windowG.LayoutSubviews();
			break;
		case "windowH":
			break;
		case "windowI":
			break;
		default:
			new Dialog("Error");
			break;
	}
}

void removeWindowDPlaySelectMessage(){
	windowD.Remove(
		windowD_messagePlayer01
	);
	windowD.Remove(
		windowD_messagePlayer02
	);
	windowD.Remove(
		windowD_messageLabel
	);
}

void nextPlayer(){
	if(gamePlayerRoute == 1){
		gamePlayerRoute = 0;
	}else{
		gamePlayerRoute = 1;
	}	
}

void playMessage(string name){
	windowD.Remove(
		windowD_playerTurnMessage
	);
	windowD.Remove(
		windowD_playerNotifyMessage
	);
	windowD_playerTurnMessage = new Label(name + ", it's your turn...", true){
		X = Pos.Left(windowD_progressBar1) + 1,
		Y = Pos.Top(windowD_progressBar1) + 3
	};
	windowD_playerTurnMessage.ColorScheme = Colors.TopLevel;
	windowD.Add(
		windowD_playerTurnMessage
	);
}

void playMessageNotify(string message){
	windowD.Remove(
		windowD_playerNotifyMessage
	);
	windowD_playerNotifyMessage = new Label(message, true){
		X = Pos.Left(windowD_progressBar1) + 1,
		Y = Pos.Top(windowD_progressBar1) + 4
	};
	windowD_playerNotifyMessage.ColorScheme = Colors.TopLevel;	
	windowD.Add(
		windowD_playerNotifyMessage
	);
}

void gamePlay(){
	playMessage(playerNames[gamePlayerRoute]);
	windowD.Add(
		windowD_playButtonFrame,
		windowD_playButton
	);
	windowD_playButton.Clicked += () =>{
		windowD.Remove(
			windowD_playerTurnMessage
		);
		windowD.Remove(
			windowD_playerNotifyMessage
		);
		if(gameRunning){
			gameRunning = false;
			Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds (100), PrepareBackgroundProcess);
		}	
	};	
}

bool PrepareBackgroundProcess (MainLoop _){
	Application.MainLoop.Invoke (async () => {
		await RunBackgroundProcessAsync ();
	});
	return false;
}

async Task RunBackgroundProcessAsync (){
	for(int j=0; j<2; j++){
		for(int i=0; i<6; i++){
			anim1 = new Label (diceAnimation[i]) { X = 4, Y = 7 };
			anim2 = new Label (diceAnimation[i]) { X = Pos.Left(anim1) + 22, Y = Pos.Top(anim1)};
			windowD.Add(
				anim1,
				anim2
			);
			await Task.Delay (170);
			windowD.Remove(
				anim1
			);
			windowD.Remove(
				anim2
			);
		}
	}
	setRandValueInToDiceFace();
}

void removeWindowCWarningMessage(){
	if(windowC_warn){
		windowC.Remove(
			windowC_warning
		);
	windowC_warn = false;
	}
}

void setDefault(){
	windowC_warn = false;
	gamePlayerRoute = 0;
	gameStart = false;
	gameRunning = true;
	player_score=new int[2]{0,0};
}

void windowRemove(){
	/* Remove all items from windows */
	windowA.RemoveAll();
	windowC.RemoveAll();
	windowD.RemoveAll();
	windowE.RemoveAll();
	windowF.RemoveAll();
	windowG.RemoveAll();
	windowC_player01TextField = new TextField(""){
		X = Pos.Right(windowC_player01Label) + 1,
		Y = Pos.Top(windowC_player01Label),
		Width = 30
	};
	windowC_player02TextField = new TextField(""){
		X = Pos.Right(windowC_player02Label) + 1,
		Y = Pos.Top(windowC_player02Label),
		Width = 30
	};
	windowD_player02Button = new Button("Play"){
		X = Pos.Right(windowD_player01Button) + 4,
		Y = Pos.Top(windowD_player01ButtonFrame) + 1,
		Width = 20,
		Height = 3
	};
}

void addDataToFile(string name, string score, string status){
	// Read CSV File
	string[] CSVDump = File.ReadAllLines(file);
	// Split Data
	List<List<string>> CSV = CSVDump.Select(x => x.Split(',').ToList()).ToList();
	// Insert new data to second row
	CSV.Insert(1, new List<string>{name, score, status}); // 1 is index of first row
	// Write all data to file again
	File.WriteAllLines(file, CSV.Select(x => string.Join(",", x)));
}

void readCSVFile(){
	DataTable dt = new DataTable();
	var lines = File.ReadAllLines(file);
	foreach(var h in lines[0].Split(',')){
		dt.Columns.Add(h);
	}
	foreach(var line in lines.Skip(1)) {
		dt.Rows.Add(line.Split(','));
	}
	windowF_tableView.Table=dt;
}

/* Set buttons functions */
// For window A
windowA_startGameButton.Clicked += () =>{
	windowReplace("windowC");
};

windowA_quitButton.Clicked += () =>{
	if (Quit ()){
		top.Running = false;
		Console.Title=consoleOldTitle; // Set console title to default after quit
	}
};

windowA_howToButton.Clicked += () =>{
	windowReplace("windowG");
};

windowA_scoreButton.Clicked += () =>{
	windowReplace("windowF");
	readCSVFile();
};

// For window E
windowE_quit.Clicked += () =>{
	if (Quit ()){
		top.Running = false;
		Console.Title=consoleOldTitle; // Set console title to default after quit
	}
};
windowE_mainMenu.Clicked += () =>{
	setDefault();
	windowRemove();
	windowAdd();
	windowReplace("windowA");
};
windowE_newGame.Clicked += () =>{
	setDefault();
	windowRemove();
	windowAdd();
	windowReplace("windowC");
};

// For window G
windowG_backToMainButton.Clicked += () =>{
	windowReplace("windowA");
};

// For window F
windowF_backToMainButton.Clicked += () =>{
	windowReplace("windowA");
};

bool askUserToDeleteFile(){
	var n = MessageBox.Query(50, 7, "Reset Score Board", "Do you want to reset score board?", "Yes", "No");
	return n == 0;
}

windowF_resetButton.Clicked += () =>{
	if(askUserToDeleteFile()){
		if(File.Exists(file)){
			File.Delete(file);
		}
		createCSVFile();
		readCSVFile();
	}
};

// For window C
windowC_backToMainButton.Clicked += () =>{
	removeWindowCWarningMessage();
	windowReplace("windowA");
};
windowC_okButton.Clicked += () =>{	
	string player02_mod;
	string player01 = windowC_player01TextField.Text.ToString();
	string player02 = windowC_player02TextField.Text.ToString();
	if(string.IsNullOrEmpty(player01) || string.IsNullOrEmpty(player02)){
		removeWindowCWarningMessage();	
		windowC_warn = true;
		windowC.Add(
			windowC_warning
		);
	}else{
		removeWindowCWarningMessage();
		windowReplace("windowD");
		/* Player name length check */
		if(player01.Length < 20){
			playerNames[0] = player01;
		}else{
			playerNames[0] = player01.Substring(0, 20);
		}
		if(player02.Length < 20){
			string space = "";
			for(int i = 0; i < (20-player02.Length); i++){
				space += " ";
			}
			player02_mod = space + player02;
			playerNames[1] = player02;
		}else{
			player02_mod = player02.Substring(0, 20);
			playerNames[1] = player02_mod;
		}
		var windowD_player1 = new Label(playerNames[0],true){
			X = 2,
			Y = 1
		};
		var windowD_player2 = new Label(player02_mod,true){
			X = Pos.Left(windowD_player1) + 25,
			Y = Pos.Top(windowD_player1)
		};
		windowD_messagePlayer01 = new Button(playerNames[0]){
			X = Pos.Left(windowD_messageLabel) + 2,
			Y = Pos.Top(windowD_messageLabel) + 3,
			Width = 24
		};
		windowD_messagePlayer01.ColorScheme = Colors.Dialog;
		windowD_messagePlayer02 = new Button(playerNames[1]){
			X = Pos.Left(windowD_messagePlayer01),
			Y = Pos.Top(windowD_messagePlayer01) + 2,
			Width = 24
		};
		windowD_messagePlayer02.ColorScheme = Colors.Dialog;
		windowD_messagePlayer01.Clicked += () =>{
			removeWindowDPlaySelectMessage();
			gamePlayerRoute = 0;
			gameStart = true;
			gamePlay();
		};
		windowD_messagePlayer02.Clicked += () =>{
			removeWindowDPlaySelectMessage();
			gamePlayerRoute = 1;
			gameStart = true;
			gamePlay();
		};
		windowD.Add(
			windowD_player1,
			windowD_player2,
			windowD_messagePlayer01,
			windowD_messagePlayer02
		);
	}	
};

/* Console menubar */
var menu = new MenuBar(new MenuBarItem[] {
	new MenuBarItem ("_Quit", "", () => {
		if (Quit ()){
		top.Running = false;
		Console.Title=consoleOldTitle; // Set console title to default
		}			
	})
});

void windowAdd(){
	/* Add items to windows */
	windowA.Add(
		windowA_logoText, 
		windowA_startGameButton, 
		windowA_scoreButton, 
		windowA_howToButton, 
		windowA_quitButton,
		windowA_version,
		windowA_author
	);
	windowC.Add(
		windowC_title,
		windowC_player01Label,
		windowC_player01TextField,
		windowC_player02Label,
		windowC_player02TextField,
		windowC_messageLabel,
		windowC_okButton,
		windowC_backToMainButton
	);
	windowD.Add(
		windowD_progressBar1,
		windowD_progressBar2,
		windowD_messageLabel
	);
	windowE.Add(
		windowE_quit,
		windowE_mainMenu,
		windowE_newGame
	);
	windowF.Add(
		windowF_title,
		windowF_tableView,
		windowF_backToMainButton,
		windowF_resetButton
	);
	windowG.Add(
		windowG_title,
		windowG_guid,
		windowG_backToMainButton
	);
}

windowAdd();
top.Add(menu);
win.Add(windowA);
Application.Run();
Application.Shutdown();