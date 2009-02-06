

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using CodeOMatic.Validation.CompileTime;

namespace CodeOMatic.Validation.CompileTime.Parser {



internal partial class SelectorParser {
	const int _EOF = 0;
	const int _symbol = 1;
	const int _iteration = 2;
	const int _memberseparator = 3;
	const int _selectorseparator = 4;
	const int maxT = 5;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public SelectorScanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;



	public SelectorParser(SelectorScanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Selectors() {
		StartParsingSelectors(); 
		Selector();
		while (la.kind == 4) {
			Get();
			Selector();
		}
	}

	void Selector() {
		SelectorPart part; 
		var parts = new List<SelectorPart>(); 
		if (la.kind == 1) {
			Member(out part);
			parts.Add(part); 
		}
		if (la.kind == 2) {
			Iteration(out part);
			parts.Add(part); 
		}
		while (la.kind == 3) {
			Get();
			Member(out part);
			parts.Add(part); 
			if (la.kind == 2) {
				Iteration(out part);
				parts.Add(part); 
			}
		}
		AddSelector(parts); 
	}

	void Member(out SelectorPart part) {
		Expect(1);
		part = new MemberSelectorPart(t.val); 
	}

	void Iteration(out SelectorPart part) {
		Expect(2);
		part = IterationSelectorPart.Instance; 
	}



	private void ParseInternal() {
		la = new Token();
		la.val = "";		
		Get();
		Selectors();

    Expect(0);
	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x}

	};
} // end SelectorParser


internal class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
  public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
  
	public void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "symbol expected"; break;
			case 2: s = "iteration expected"; break;
			case 3: s = "memberseparator expected"; break;
			case 4: s = "selectorseparator expected"; break;
			case 5: s = "??? expected"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


internal class FatalError: Exception {
	public FatalError(string m): base(m) {}
}

#pragma warning restore 1591

}