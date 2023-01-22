using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;


public class pilot : MonoBehaviour
{
  private CharacterController controller;
  private Vector3 playerVelocity;
  private bool groundedPlayer;
  private float playerSpeed = 2.0f;
  private float jumpHeight = 1.0f;
  private float gravityValue = -9.81f;
  KeywordRecognizer keywordRecognizer;
  Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
  
  // Start is called before the first frame update
  private void Start()
  {
    controller = gameObject.AddComponent<CharacterController>();
  //Create keywords for keyword recognizer
  keywords.Add("activate", () =>
    {
      // action to be performed when this keyword is spoken
      Vector3 move = new Vector3(
        Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")
        );
      controller.Move(move * Time.deltaTime * playerSpeed);
    });
    
  }

    // Update is called once per frame
  void Update()
  {
    keywordRecognizer.Start();
    keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
    keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

    groundedPlayer = controller.isGrounded;
    if (groundedPlayer && playerVelocity.y < 0)
    {
      playerVelocity.y = 0f;
    }

    Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    controller.Move(move * Time.deltaTime * playerSpeed);

    if (move != Vector3.zero)
    {
      gameObject.transform.forward = move;
    }

    // Changes the height position of the player..
    if (Input.GetButtonDown("Jump"))
    {
      playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    playerVelocity.y += gravityValue * Time.deltaTime;
    controller.Move(playerVelocity * Time.deltaTime);
  }

  private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
  {
      System.Action keywordAction;
      // if the keyword recognized is in our dictionary, call that Action.
      if (keywords.TryGetValue(args.text, out keywordAction))
      {
          keywordAction.Invoke();
      }
  }



}


