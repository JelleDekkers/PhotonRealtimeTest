open PhotonControl in Photon Realtime Test\Photon-OnPremise-Server-SDK_v4-0-29-11263\deploy\bin_Win64
dit voegt PhotonControl toe als pictogram in je taskbar
klik hierop en klik op Game Server Ip Config -> Set Local IP x.x.x.x (Onthoud wat er hier staat voor straks)
Klik vervolgens bij PhotonControl op LoadBalancing (my cloud) -> start as application
in unity open SampleScene en start, zet het juiste IP adress van daarnet en druk op Connect en Create & Join Room, als het goed gaat zie je nu CurrentRoom player count: 1
Met een echo project of build kun je nu verbinden (of een andere room starten) en zal de player count omhoog gaan

Om PhotonControl af te sluiten, druk eerst op LoadBalancing -> Stop Application en dan Exit Photon Control