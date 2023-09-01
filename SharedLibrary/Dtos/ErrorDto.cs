using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos;

public class ErrorDto
{
	public ErrorDto(string error, bool isShow)
	{
		Errors.Add(error);
		IsShow = isShow;
	}

	public ErrorDto(List<string> errors, bool isShow)
	{
		Errors = errors;
		IsShow = isShow;
	}

	// private set --> qiraqdan deyisikliyin qarsisini aliriq, ancaq constructor ile set ede bilerler
	public List<string> Errors { get; private set; } = new List<string>();
	public bool IsShow { get; private set; }
}