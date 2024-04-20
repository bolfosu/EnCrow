Privacy-Preserving Age Verification Using Zero-Knowledge Proofs


1. Background
Proving age is required in many scenarios, such as purchasing age-restricted items, gaining entry to venues, or receiving service discounts based on certain age limits. However, traditional methods often necessitate the disclosure of sensitive personal information. For example, when an ID is presented as proof of age, it typically reveals not only the age but also the name and address, posing privacy risks. To address this issue, it’s proposed to develop an app-based solution that utilizes the Danish MitID system to obtain identity data. This system will then generate a QR code for age verification without the need to reveal personal identification details. The project  aims to use zero-knowledge proofs (ZKPs) to enable age verification without disclosing the exact age or any other personal data. This approach improves privacy and security while still can serve as a trustworthy proof of age.

2. Method
Proposed method utilizes Schnorr protocol with Fiat-Shamir Heuristic, a cryptographic protocol that allows one party (the prover) to prove the validity of a statement to another party (the verifier) without revealing any additional information beyond the truth of the statement itself. The method involves the following steps:

	Age Commitment:
	Provers authenticate once with Danish MitID to enable age verification.
	Upon authentication, the prover generates a commitment to their age using the Schnorr Protocol with Fiat-Shamir Heuristic. This commitment hides the prover's actual age while enabling verification.
	Proof Generation:
	Using the committed age, the prover computes a challenge value based on a cryptographic hash function, such as SHA-256.
	The prover then calculates a response value derived from the challenge and their private key, without revealing the private key itself. This response serves as the proof of age.
	Encoding into QR Code:
	Once the proof of age is computed, it is encoded into a QR code along with other relevant data. 
	Verification:
	The verifier scans the QR code using their app and extracts the commitment and other data like timestamp.
	The verifier checks the proof  provided in the QR code 
	If the equation holds valid, the verifier accepts the proof, confirming that the prover is older than 18 years old without learning the exact age.


In addition to utilizing zero-knowledge proofs (ZKPs) for privacy preservation, this project aims to incorporate other security methods and techniques such as:

	Selfie verification: Provers take a selfie after MitID authentication like a proof that the verified age belongs to them, this picture is shown to verifier along with QR code.
	Privacy preservation: Provers picture is stored and displayed only on their device to maintain data privacy.
	App authentication: Prover uses 6 digits pin code to use the app.
	Session control:  QR code is uniquely generated and include a time stamp to minimalize risk of passing QR code to another person or the risk of unauthorized access.
	Non-Interactive: The verification session is done just in one step by scanning QR code without dynamic interactions.
	Autonomy: Verification can be done without internet access.




2.1 ZK proof protocols
After analyzing different ZKP protocols, the choice was narrowed down to two options, Bulletproofs and the Schnorr Protocol using the Fiat-Shamir Heuristic. Although Bulletproofs are more suitable for the project's scope because it can prove knowledge of a range, such as proving that the prover's age falls within certain values, a non – dynamic Schnorr protocol was ultimately chosen due to the following reasons:

Complexity of Implementation:
	Schnorr Protocol with Fiat-Shamir Heuristic involves fewer steps and computations compared to Bulletproofs. It mainly requires generating commitments, computing hash values, and performing modular arithmetic.
	Bulletproofs, on the other hand, involve more complex mathematical operations such as inner product commitments, polynomial equation construction, and range proofs, which can be more complicated to implement correctly.
Verification Process:
	The verification process for the Schnorr Protocol with Fiat-Shamir Heuristic is relatively straightforward. Verifying the commitment and challenge involves comparing hash values and performing modular arithmetic checks.
	Bulletproofs require more extensive verification procedures, including multiple mathematical checks to ensure the validity of the proof.
Resource Efficiency:
	Schnorr Protocol with Fiat-Shamir Heuristic typically requires fewer computational resources compared to Bulletproofs, making it more lightweight in terms of processing power and memory usage.





2.2 Protocol implementation
Pre shared values 
	Public Parameters:
	Prime modulus p=11.
	Generator g=5.
	Prime order q∣(p-1)
	Cryptographic hash function 𝐻:ℤ𝑅⟵{0,1}∗ 



Public Key:
	Age that needs to be proven is hashed and act as private key
	H(w)=w mod 17
	Public key  h≡g^w  mod p  

Protocol execution.

Commitment Phase:
  - Prover chooses a random value r and computes the commitment a as a ≡ g^r mod p
  - Prover computes the hash c=H(a)
  - Prover computes  z=r+wc mod q
  - Prover sends a and z  to Verifier

Verification Phase:
- Verifier checks if g^R≡a×h^(w  ) mod p
- If the equation holds, the verifier accepts the proof, otherwise it rejects it.


	

