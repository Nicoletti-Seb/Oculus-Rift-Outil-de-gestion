#pragma strict

var username : String;
var inputField : UnityEngine.UI.InputField;
var nameDisplay : UnityEngine.UI.Text;

function Update () {
    username = inputField.text;
    nameDisplay.text = username;
}