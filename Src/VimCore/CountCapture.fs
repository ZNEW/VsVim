﻿#light

namespace Vim
open Microsoft.VisualStudio.Text
open Microsoft.VisualStudio.Text.Editor;

module internal CountCapture =

    /// Get a count value starting with the provided KeyInput
    let GetCount keyRemapMode (keyInput: KeyInput) = 
        let charToInt (c: char) = c.ToString() |> System.Int32.Parse

        let rec nextFunc number (keyInput: KeyInput) =
            match keyInput.IsDigit with
            | true ->
                let digit = charToInt keyInput.Char
                let number = (number * 10) + digit
                let bindData = { KeyRemapMode = keyRemapMode; BindFunction = nextFunc number }
                BindResult<_>.NeedMoreInput bindData
            | false ->
                BindResult.Complete (Some number, keyInput)

        match keyInput.IsDigit && keyInput.Char <> '0' with
        | true -> 
            let number = charToInt keyInput.Char
            let bindData = { KeyRemapMode = keyRemapMode; BindFunction = nextFunc number }
            BindResult<_>.NeedMoreInput bindData
        | false ->
            BindResult.Complete (None, keyInput)

