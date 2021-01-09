using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

namespace MouseTrap {
    public class Build : MonoBehaviour
    {
        public GameObject ball1;
        public GameObject ball2;
        public GameObject cage;
        public GameObject diver;
        public GameObject crank;
        public GameObject baseA;
        public GameObject gearSupport;
        public GameObject gear3;
        public GameObject gear5;
        public GameObject lampPost;
        public GameObject shoe;
        public GameObject stopSign;
        public GameObject baseAPole;
        public GameObject ramp;
        public GameObject bucket;
        public GameObject baseB;
        public GameObject chute;
        public GameObject hand;
        public GameObject handRod;
        public GameObject part15;
        public GameObject bathtub;
        public GameObject teeterTotter;
        public GameObject baseC;
        public GameObject tub;
        public GameObject pivotAxle;
        public GameObject pipes;
        public GameObject post;
        public GameObject trussFront;
        public GameObject trussBack;
        public int numBuilt = 0;
        public bool buildAnother = false;
        public GameObject buildPanel;

        public GameObject[][] buildOrder;
        void Start()
        {
            SetBuildGroups();
        }

        public void PlaceBuild()
        {
            buildPanel.SetActive(false);
            foreach (GameObject obj in buildOrder[numBuilt]) {
                obj.SetActive(true);
            }

            numBuilt++;
            PhotonEvents.SendEvent(PhotonEvents.NumBuiltCode, numBuilt);

            if (buildAnother) {
                GameManager.instance.OpenBuild(1);
                buildAnother = false;
            }
            else {
                GameManager.instance.EnableButton("endTurnButton");
            }
        }

        public void LocalBuild(int newNumBuilt)
        {
            int numTimes = newNumBuilt - numBuilt;
            for (int i = 0; i < numTimes; i++) {
                foreach (GameObject obj in buildOrder[numBuilt]) {
                    obj.SetActive(true);
                }
                numBuilt++; // Also update numBuilt
            }
        }
        
        private void SetBuildGroups()
        {
            GameObject[] group1 = new GameObject[] {baseA};
            GameObject[] group2 = new GameObject[] {gearSupport};
            GameObject[] group3 = new GameObject[] {gear3};
            GameObject[] group4 = new GameObject[] {crank};
            GameObject[] group5 = new GameObject[] {gear5};
            GameObject[] group6 = new GameObject[] {stopSign, baseAPole};
            GameObject[] group7 = new GameObject[] {lampPost};
            GameObject[] group8 = new GameObject[] {shoe};
            GameObject[] group9 = new GameObject[] {trussFront, trussBack, ramp};
            GameObject[] group10 = new GameObject[] {bucket, ball1};
            GameObject[] group11 = new GameObject[] {baseB};
            GameObject[] group12 = new GameObject[] {chute};
            GameObject[] group13 = new GameObject[] {pipes, pivotAxle};
            GameObject[] group14 = new GameObject[] {handRod, hand};
            GameObject[] group15 = new GameObject[] {part15};
            GameObject[] group16 = new GameObject[] {bathtub};
            GameObject[] group17 = new GameObject[] {ball2};
            GameObject[] group18 = new GameObject[] {teeterTotter};
            GameObject[] group19 = new GameObject[] {diver};
            GameObject[] group20 = new GameObject[] {baseC};
            GameObject[] group21 = new GameObject[] {tub};
            GameObject[] group22 = new GameObject[] {post};
            GameObject[] group23 = new GameObject[] {cage};

            buildOrder = new GameObject[][] {group1, group2, group3, group4, group5, group6, group7, group8, group9, group10, group11, group12, group13, group14, group15, group16, group17, group18, group19, group20, group21, group22, group23};
        }

        public void ResetAll()
        {
            foreach(GameObject[] group in buildOrder) {
                foreach(GameObject obj in group) {
                    if (obj.activeSelf) {
                        ResetPosition resetPos = obj.GetComponent<ResetPosition>();
                        AddForceUp addForceUp = obj.GetComponent<AddForceUp>();
                        Cage cageComp = obj.GetComponent<Cage>();
                        Diver diverComp = obj.GetComponent<Diver>();
                        IncreaseGravity increaseGravity = obj.GetComponent<IncreaseGravity>();

                        if (resetPos != null) {
                            PhotonView photonView = obj.GetComponent<PhotonView>();
                            if (photonView != null) {
                                if (photonView.IsMine) {
                                    resetPos.ResetPos();
                                }
                            }
                            else {
                                resetPos.ResetPos();
                            }
                        }
                        if (addForceUp != null) {
                            addForceUp.triggerActive = true;
                            Debug.Log("addforceup");
                        }
                        if (cageComp != null) {
                            cageComp.ResetStart();
                            Debug.Log("cagecomp");
                        }
                        if (diverComp != null) {
                            diverComp.Reset();
                            Debug.Log("diverComp");
                        }
                        if (increaseGravity != null) {
                            increaseGravity.triggered = false;
                            Debug.Log("increaseGravity");
                        }
                    }
                }
            }
        }
    }
}