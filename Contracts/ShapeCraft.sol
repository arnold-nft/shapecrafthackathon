// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

contract ChatAccessNFT is ERC721URIStorage, Ownable {
    uint256 public constant FEE = 0.005 ether;
    uint256 public constant DAILY_LIMIT = 100;

    struct UserAccess {
        uint256 validUntil;   // timestamp (1 day access)
        uint256 requestCount; // API requests made in current period
        uint256 txCount;      // how many times paid
        bool canMint;         // eligible to mint NFT
    }

    mapping(address => UserAccess) public users;
    uint256 public tokenIdCounter;

    constructor() ERC721("ChatAccessNFT", "CAI") Ownable(msg.sender) {}

    function buyAccess() external payable {
        require(msg.value == FEE, "Must pay 0.005 ETH");

        UserAccess storage user = users[msg.sender];

        user.validUntil = block.timestamp + 1 days;
        user.requestCount = 0;
        user.txCount++;

        if (user.txCount >= 5) {
            user.canMint = true;
        }
    }

    function hasAccess(address userAddr) external view returns (bool) {
        UserAccess memory user = users[userAddr];
        return block.timestamp <= user.validUntil && user.requestCount < DAILY_LIMIT;
    }

    function consumeRequest(address userAddr) external onlyOwner {
        require(block.timestamp <= users[userAddr].validUntil, "Access expired");
        require(users[userAddr].requestCount < DAILY_LIMIT, "Limit reached");
        users[userAddr].requestCount++;
    }

    function mintRewardNFT(string memory tokenURI) external {
        UserAccess storage user = users[msg.sender];
        require(user.canMint, "Not eligible to mint yet");

        tokenIdCounter++;
        _safeMint(msg.sender, tokenIdCounter);
        _setTokenURI(tokenIdCounter, tokenURI);

        user.canMint = false;
    }

    function withdraw() external onlyOwner {
        payable(owner()).transfer(address(this).balance);
    }
}
