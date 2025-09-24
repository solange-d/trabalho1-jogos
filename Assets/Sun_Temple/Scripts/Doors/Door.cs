using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
   

    public class Door : MonoBehaviour
    {
		public bool IsLocked = false;
        public bool DoorClosed = true;
        public float OpenRotationAmount = 90;
        public float RotationSpeed = 1f;
        public float MaxDistance = 3.0f;
		public string playerTag = "Player";
		private Collider DoorCollider;

		private GameObject Player;
		private Camera Cam;
		private CursorManager cursor;

        Vector3 StartRotation;
        float StartAngle = 0;
        float EndAngle = 0;
        float LerpTime = 1f;
        float CurrentLerpTime = 0;
        bool Rotating;


		private bool scriptIsEnabled = true;

        [Header("Feedback")]
        public AudioClip unlockSound;
        private AudioSource audioSource;   

        public Renderer doorRenderer;      
        public Color highlightColor = Color.yellow;
        private Color originalColor;


        void Start(){
            StartRotation = transform.localEulerAngles ;
			DoorCollider = GetComponent<BoxCollider> ();

			if (!DoorCollider) {
				Debug.LogWarning (this.GetType ().Name + ".cs on " + gameObject.name + "door has no collider", gameObject);
				scriptIsEnabled = false;
				return;
			}

			Player = GameObject.FindGameObjectWithTag (playerTag);

			if (!Player) {
				Debug.LogWarning (this.GetType ().Name + ".cs on " + this.name + ", No object tagged with " + playerTag + " found in Scene", gameObject);
				scriptIsEnabled = false;
				return;
			}

			Cam = Camera.main;
			if (!Cam) {
				Debug.LogWarning (this.GetType ().Name + ", No objects tagged with MainCamera in Scene", gameObject);
				scriptIsEnabled = false;
			}
		
			cursor = CursorManager.instance;

			if (cursor != null) {
				cursor.SetCursorToDefault ();
			}

            audioSource = GetComponent<AudioSource>();

            if (doorRenderer != null)
                originalColor = doorRenderer.material.color;


        }



		void Update()
		{
			if (scriptIsEnabled) {
				if (Rotating) {
					Rotate ();
				}

				if (Input.GetButtonDown ("OpenDoor")) {
					TryToOpen ();
				}


				if (cursor != null) {
					CursorHint ();
				}
			}

		} 




		void TryToOpen(){
			if (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) <= MaxDistance){	

				Ray ray = Cam.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
				RaycastHit hit;
											
				if (DoorCollider.Raycast(ray, out hit, MaxDistance)){					
					if (IsLocked == false){
						Activate ();
					}
				}
			}
		}



		void CursorHint(){
			if (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) <= MaxDistance){	
				Ray ray = Cam.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
				RaycastHit hit;

				if (DoorCollider.Raycast (ray, out hit, MaxDistance)) {
					if (IsLocked == false) {
						cursor.SetCursorToDoor ();
					} else if (IsLocked == true) {
						cursor.SetCursorToLocked ();
					}					
				} else {
					cursor.SetCursorToDefault ();
				}
			}
		}




        public void Activate()
        {
            if (DoorClosed)
                Open();
            else
                Close();
        }



       



        void Rotate()
        {
            CurrentLerpTime += Time.deltaTime * RotationSpeed;
            if (CurrentLerpTime > LerpTime)
            {
                CurrentLerpTime = LerpTime;
            }

            float _Perc = CurrentLerpTime / LerpTime;

            float _Angle = CircularLerp.Clerp(StartAngle, EndAngle, _Perc);
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, _Angle, transform.eulerAngles.z);

			if (CurrentLerpTime == LerpTime) {
				Rotating = false;
				DoorCollider.enabled = true;
			}
              
           
        }



        void Open()
        {
			DoorCollider.enabled = false;
            DoorClosed = false;
            StartAngle = transform.localEulerAngles.y;
            EndAngle =  StartRotation.y + OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
        }



        void Close()
        {
			DoorCollider.enabled = false;
            DoorClosed = true;
            StartAngle = transform.localEulerAngles.y;
            EndAngle = transform.localEulerAngles.y - OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
        }

        public void UnlockWithFeedback()
        {
            IsLocked = false;

            // Tocar som
            if (audioSource != null && unlockSound != null)
                audioSource.PlayOneShot(unlockSound);

            // Iluminar a porta
            if (doorRenderer != null)
                doorRenderer.material.color = highlightColor;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag) && !IsLocked)
            {
                if (doorRenderer != null)
                    doorRenderer.material.color = originalColor;
            }
        }

    }
}