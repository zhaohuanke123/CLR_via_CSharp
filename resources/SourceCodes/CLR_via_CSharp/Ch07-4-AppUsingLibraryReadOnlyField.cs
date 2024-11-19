using System;
using LibraryDefiningReadOnlyField;

public sealed class Program {
	static void Main() {
		Console.WriteLine("Max entries supported in list: "
			+ SomeLibraryType.MaxEntriesInList);
	}
}