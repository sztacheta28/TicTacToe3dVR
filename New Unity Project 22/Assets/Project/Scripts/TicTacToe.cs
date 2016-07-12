using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TicTacToe : MonoBehaviour {
	char gracz, wybor;
	char[] t = new char[10];

	public Transform[] spawns = new Transform[10];

	public GameObject circle;
	public GameObject sharp;

	public GameObject plansza;
	public GameObject detonator;

	List<GameObject> kolkaIKrzyzyki = new List<GameObject>();

	string communicat = "";

	//-------------------------------------
	void Start()
	{
		gracz = 'O';

		for (int i = 1; i <= 9; i++) t[i] = ' ';
	}

	//-------------------------------------
	void Update()
	{
		string test = "";

		for(int i=1; i<=9; ++i) test+=t[i]+"|";

		print(test);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

	//-------------------------------------
	public void MainFunc(int ruch)
	{		
		if(!czyKoniec(t)) wykonajRuch(t, ref gracz, ruch);
		if(!czyKoniec(t)) wykonajRuch(t, ref gracz);
	}

	// Funkcja zwraca true jeśli nastąpiła wygrana jednego z zawodników albo remis
	//-------------------------------------
	bool czyKoniec(char[] t)
	{
		bool koniec = true;
		koniec &= !(czyWygrana(t, 'X', false));
		koniec &= !(czyWygrana(t, 'O', false));
		koniec &= !(czyRemis(t, false));
		
		return !koniec;
	}
	
	// Funkcja zwraca true, jeśli nastąpiła wygrana danego zawodnika
	//-------------------------------------
	bool czyWygrana(char[] t, char g, bool czyWyswietlac)
	{
		bool test;
		int i;
		
		test = false; 
		
		for (i = 1; i <= 7; i += 3) test |= ((t[i] == g) && (t[i + 1] == g) && (t[i + 2] == g));
		for (i = 1; i <= 3; i++) test |= ((t[i] == g) && (t[i + 3] == g) && (t[i + 6] == g));
		test |= ((t[1] == g) && (t[5] == g) && (t[9] == g));
		test |= ((t[3] == g) && (t[5] == g) && (t[7] == g));
		
		if (test) //jeśli wygrałem
		{
			if (!czyWyswietlac)
			{
				communicat = "PRZEGRAŁEŚ";
			}
			return true;
		}
		return false;
	}
	
	// Funkcja zwraca true, jeśli na planszy nie ma już żadnego wolnego pola na wykonajRuch.
	//-------------------------------------------------
	bool czyRemis(char[] t, bool czyWyswietlac)
	{
		
		for (int i = 1; i <= 9; i++) if (t[i] == ' ') return false;
		
		if (!czyWyswietlac)
		{
			communicat = "REMIS :)";
		}
		
		return true;
	}
	
	// Algorytm rekurencyjny MINIMAX
	//----------------------------------------------------------------
	int minimax(char[] t, char gracz)
	{
		int m, mmx;
		
		if (czyWygrana(t, gracz, true)) return (gracz == 'X') ? 1 : -1;
		
		if (czyRemis(t, true) == true) return 0;
		
		gracz = (gracz == 'X') ? 'O' : 'X';
		
		mmx = (gracz == 'O') ? 2 : -2;
		
		// X - wyznaczamy maximum
		// O - wyznaczamy minimum
		for (int i = 1; i <= 9; i++)
			if (t[i] == ' ')
		{
			t[i] = gracz;
			m = minimax(t, gracz);
			t[i] = ' ';
			if (((gracz == 'O') && (m < mmx)) || ((gracz == 'X') && (m > mmx))) mmx = m;
		}
		return mmx;
	}
	
	// Funkcja wyznacza ruch dla komputera.
	//------------------------------------
	int ruchKomputera(char[] t)
	{
		int wykonajRuch = 0, m, mmx;
		
		mmx = -2;
		for (int i = 1; i <= 9; i++)
			if (t[i] == ' ')
		{
			t[i] = 'X';
			m = minimax(t, 'X');
			t[i] = ' ';
			if (m > mmx) { 
				mmx = m;
				wykonajRuch = i;
			}
		}
		return wykonajRuch;
	}
	
	// Funkcja umożliwia ruch gracza
	// Po ruchu następuje zamiana gracza
	//------------------------------------
	void wykonajRuch(char[] t, ref char gracz, int r = 0)
	{
		if (gracz == 'X')
		{
			r = ruchKomputera(t);
		}
		
		t[r] = gracz;

		UstawNaPlanszy(r);

		gracz = (gracz == 'O') ? 'X' : 'O';
	}

	//------------------------------------
	void UstawNaPlanszy(int r)
	{
		if(gracz == 'X'){
			GameObject exp = (GameObject) Instantiate(detonator, spawns[r].position, Quaternion.identity);
			Destroy(exp, 3);
			GameObject go = Instantiate(sharp, spawns[r].position, Quaternion.identity) as GameObject;
			go.transform.parent = plansza.transform;
			kolkaIKrzyzyki.Add(go);
			spawns[r].GetComponent<Collider>().enabled = false;
		}else{
			GameObject exp = (GameObject) Instantiate(detonator, spawns[r].position, Quaternion.identity);
			Destroy(exp, 3);
			GameObject go = Instantiate(circle, spawns[r].position, Quaternion.identity) as GameObject;
			go.transform.parent = plansza.transform;
			kolkaIKrzyzyki.Add(go);
		}
	}

	//------------------------------------
	void Restart()
	{
		print ("RESTART");
		foreach(GameObject go in kolkaIKrzyzyki)
			Destroy(go);

		kolkaIKrzyzyki.Clear();

		gracz = 'O';
		
		for (int i = 1; i <= 9; i++) t[i] = ' ';

		for(int i = 1; i<spawns.Length; ++i){
			spawns[i].GetComponent<Collider>().enabled = true;
		}

		communicat = "";
	}

	//------------------------------------
	void OnGUI()
	{
		if(czyKoniec(t)){
			GUI.Label(new Rect(Screen.width/2-100, Screen.height/2-190, 280, 70), "<b><color=purple><size=40>"+communicat+"</size></color></b>");
			if(GUI.Button(new Rect(Screen.width/2-225, Screen.height/2-90, 450, 180), "<size=55>Restart</size>")){
				Restart();
			}
		}
	}
}
