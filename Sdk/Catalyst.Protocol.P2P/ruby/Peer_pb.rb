# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: Peer.proto

require 'google/protobuf'

Google::Protobuf::DescriptorPool.generated_pool.build do
  add_message "Catalyst.Protocol.Peer.PeerProtocol" do
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.PingRequest" do
    optional :ping, :string, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.PongResponse" do
    optional :pong, :string, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.ACK" do
    optional :ack, :bytes, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.NACK" do
    optional :nack, :bytes, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.PeerInfoRequest" do
    optional :ping, :string, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.PeerInfoResponse" do
    optional :pong, :string, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.PeerNeighborsRequest" do
    optional :ping, :string, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.PeerNeighborsResponse" do
    optional :pong, :string, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.ChallengeRequest" do
    optional :nonce, :sint64, 1
  end
  add_message "Catalyst.Protocol.Peer.PeerProtocol.ChallengeResponse" do
    optional :signedNonce, :string, 1
    optional :publicKey, :string, 2
  end
end

module Catalyst
  module Protocol
    module Peer
      PeerProtocol = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol").msgclass
      PeerProtocol::PingRequest = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.PingRequest").msgclass
      PeerProtocol::PongResponse = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.PongResponse").msgclass
      PeerProtocol::ACK = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.ACK").msgclass
      PeerProtocol::NACK = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.NACK").msgclass
      PeerProtocol::PeerInfoRequest = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.PeerInfoRequest").msgclass
      PeerProtocol::PeerInfoResponse = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.PeerInfoResponse").msgclass
      PeerProtocol::PeerNeighborsRequest = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.PeerNeighborsRequest").msgclass
      PeerProtocol::PeerNeighborsResponse = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.PeerNeighborsResponse").msgclass
      PeerProtocol::ChallengeRequest = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.ChallengeRequest").msgclass
      PeerProtocol::ChallengeResponse = Google::Protobuf::DescriptorPool.generated_pool.lookup("Catalyst.Protocol.Peer.PeerProtocol.ChallengeResponse").msgclass
    end
  end
end